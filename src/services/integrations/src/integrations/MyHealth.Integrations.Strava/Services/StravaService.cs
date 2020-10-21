using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyHealth.Extensions.AspNetCore.Context;
using MyHealth.Extensions.Caching.Distributed;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Events;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public class StravaService : IStravaService, IIntegrationSystemService
    {
        private readonly StravaSettings _stravaSettings;
        private readonly IStravaClient _stravaClient;
        private readonly IEventPublisher _eventPublisher;
        private readonly IOperationContext _operationContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDistributedCache _cache;
        private readonly ILogger<StravaService> _logger;

        public StravaService(
            IOptions<StravaSettings> stravaSettings,
            IStravaClient stravaClient,
            IEventPublisher eventPublisher,
            IOperationContext operationContext,
            IDateTimeProvider dateTimeProvider,
            IDistributedCache cache,
            ILogger<StravaService> logger)
        {
            _stravaSettings = stravaSettings.Value;
            _stravaClient = stravaClient;
            _eventPublisher = eventPublisher;
            _operationContext = operationContext;
            _dateTimeProvider = dateTimeProvider;
            _cache = cache;
            _logger = logger;
        }

        public Provider Provider => Provider.Strava;

        public async Task<ProviderResult> CreateIntegrationAsync(ProviderRequest request)
        {
            var requestData = (CreateStravaIntegrationRequest)request.Data;
            TokenResponse tokenResponse = await _stravaClient.AuthenticateAsync(requestData.Code);

            return new ProviderResult
            {
                Provider = Provider,
                ProviderUserId = tokenResponse.Athlete.Id.ToString(),
                Data = new StravaIntegrationData
                {
                    AccessToken = tokenResponse.AccessToken,
                    AccessTokenExpiresUtc = DateTimeOffset.FromUnixTimeSeconds(tokenResponse.ExpiresAt).UtcDateTime,
                    RefreshToken = tokenResponse.RefreshToken
                }
            };
        }

        public async Task<StravaSubscription> CreateSubscriptionAsync(string callbackUrl)
        {
            StravaSubscription subscription = await _stravaClient.CreateSubscriptionAsync(callbackUrl);
            await CacheSubscriptionAsync(subscription);

            return subscription;
        }

        public Task DeleteIntegrationAsync(string userId)
        {
            // nothing to do - the Strava subscription is for the application, not the user
            return Task.CompletedTask;
        }

        public async Task DeleteSubscriptionAsync()
        {
            StravaSubscription subscription = await GetSubscriptionAsync();

            if (subscription != null)
            {
                await _stravaClient.DeleteSubscriptionAsync(subscription.Id);
                await _cache.RemoveAsync(Cache.StravaSubscription);
            }
        }

        public string GetAuthenticationUri(string redirectUri)
        {
            return QueryHelpers.AddQueryString(
                _stravaSettings.AuthenticationUrl,
                new Dictionary<string, string>
                {
                    { "client_id", _stravaSettings.ClientId },
                    { "response_type", "code" },
                    { "redirect_uri", redirectUri },
                    { "approval_prompt", "force" },
                    { "scope", "activity:read" }
                });
        }

        public async Task<StravaSubscription> GetSubscriptionAsync()
        {
            var cached = await _cache.GetAsync<StravaSubscription>(Cache.StravaSubscription);

            if (cached != null)
            {
                return cached;
            }

            IEnumerable<StravaSubscription> subscriptions = await _stravaClient.GetSubscriptionsAsync();
            StravaSubscription subscription = subscriptions.FirstOrDefault();

            await CacheSubscriptionAsync(subscription);

            return subscription;
        }

        public async Task ProcessUpdateNotification(StravaUpdateNotification updateNotification)
        {
            StravaSubscription subscription = await GetSubscriptionAsync();

            if (updateNotification.SubscriptionId != subscription.Id)
            {
                _logger.LogWarning($"Received Strava update for unrecognised subscription ID: {updateNotification.SubscriptionId}");
                return;
            }

            await _eventPublisher.PublishAsync(
                new IntegrationProviderUpdateEvent(
                    id: Guid.NewGuid().ToString(),
                    subject: updateNotification.OwnerId.ToString(),
                    eventTime: _dateTimeProvider.UtcNow,
                    dataVersion: EventConstants.EventDataVersion,
                    data: new IntegrationProviderEventData
                    {
                        OperationId = _operationContext.OperationId,
                        SourceSystem = EventConstants.IntegrationsApi,
                        SubjectSystem = Provider.Strava.ToString(),
                        Provider = Provider,
                        ProviderData = updateNotification
                    }));
        }

        public bool ValidateSubscription(string verifyToken)
        {
            return verifyToken == _stravaSettings.SubscriptionVerifyToken;
        }

        private async Task CacheSubscriptionAsync(StravaSubscription subscription) =>
            await _cache.SetAsync(
                Cache.StravaSubscription,
                subscription,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
    }
}
