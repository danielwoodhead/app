using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public class StravaService : IStravaService, IIntegrationSystemService
    {
        private readonly StravaSettings _stravaSettings;
        private readonly IStravaClient _stravaClient;

        public StravaService(
            IOptions<StravaSettings> stravaSettings,
            IStravaClient stravaClient)
        {
            _stravaSettings = stravaSettings.Value;
            _stravaClient = stravaClient;
        }

        public Provider Provider => Provider.Strava;

        public async Task<ProviderResult> CreateIntegrationAsync(ProviderRequest request)
        {
            var requestData = (CreateStravaIntegrationRequest)request.Data;
            TokenResponse tokenResponse = await _stravaClient.AuthenticateAsync(requestData.Code);

            return new ProviderResult
            {
                Provider = Provider,
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
            return await _stravaClient.CreateSubscriptionAsync(callbackUrl);
        }

        public Task DeleteIntegrationAsync(string userId)
        {
            // nothing to do - the Strava subscription is for the application, not the user
            return Task.CompletedTask;
        }

        public async Task DeleteSubscriptionAsync()
        {
            var subscriptions = await GetSubscriptionsAsync();

            if (subscriptions != null && subscriptions.Any())
            {
                await _stravaClient.DeleteSubscriptionAsync(subscriptions.First().Id);
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
                    { "scope", "read" }
                });
        }

        public async Task<IEnumerable<StravaSubscription>> GetSubscriptionsAsync()
        {
            return await _stravaClient.GetSubscriptionsAsync();
        }

        public bool ValidateSubscription(string verifyToken)
        {
            return verifyToken == _stravaSettings.SubscriptionVerifyToken;
        }
    }
}
