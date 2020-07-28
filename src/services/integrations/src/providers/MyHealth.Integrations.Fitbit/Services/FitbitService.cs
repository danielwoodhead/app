using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using MyHealth.Integrations.Models.Requests;
using MyHealth.Integrations.Utility;

namespace MyHealth.Integrations.Fitbit.Services
{
    public class FitbitService : IFitbitService
    {
        private const string EventDataVersion = "1.0";

        private readonly IEventPublisher _eventPublisher;
        private readonly IOperationContext _operationContext;
        private readonly IFitbitClient _fitbitClient;
        private readonly IIntegrationsService _integrationsService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly FitbitSettings _fitBitSettings;

        public FitbitService(
            IEventPublisher eventPublisher,
            IOperationContext operationContext,
            IFitbitClient fitbitClient,
            IIntegrationsService integrationsService,
            IDateTimeProvider dateTimeProvider,
            IOptions<FitbitSettings> fitBitSettings)
        {
            _eventPublisher = eventPublisher;
            _operationContext = operationContext;
            _fitbitClient = fitbitClient;
            _integrationsService = integrationsService;
            _dateTimeProvider = dateTimeProvider;
            _fitBitSettings = fitBitSettings.Value;
        }

        public async Task CreateIntegrationAsync(string userId, AuthorizationCodeRequest request)
        {
            TokenResponse tokenResponse = await _fitbitClient.AuthenticateAsync(request.Code, request.RedirectUri);

            if (tokenResponse.IsError)
                throw new Exception(tokenResponse.Error); // TODO: if invalid code - return 4XX else throw exception

            // TODO: await _fitbitClient.AddSubscriptionAsync(subscriptionId: userId);

            await _integrationsService.CreateIntegrationAsync(
                new CreateIntegrationRequest
                {
                    Provider = Provider.Fitbit,
                    Data = new FitbitIntegrationData
                    {
                        AccessToken = tokenResponse.AccessToken,
                        AccessTokenExpiresUtc = _dateTimeProvider.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
                        RefreshToken = tokenResponse.RefreshToken
                    }
                });
        }

        public async Task ProcessUpdateNotificationAsync(IEnumerable<FitbitUpdateNotification> request)
        {
            // TODO: signature verification

            await _eventPublisher.PublishAsync(request.Select(update =>
                new IntegrationProviderUpdateEvent(
                    id: Guid.NewGuid().ToString(),
                    subject: update.SubscriptionId,
                    eventTime: _dateTimeProvider.UtcNow,
                    dataVersion: EventDataVersion,
                    data: new IntegrationEventData
                    {
                        OperationId = _operationContext.OperationId,
                        SourceSystem = FitbitConstants.Provider,
                        SubjectSystem = FitbitConstants.SubscriberSystem,
                        Provider = FitbitConstants.Provider,
                        UserId = update.SubscriptionId
                    })));
        }

        public bool Verify(string verificationCode)
        {
            return verificationCode == _fitBitSettings.VerificationCode;
        }
    }
}
