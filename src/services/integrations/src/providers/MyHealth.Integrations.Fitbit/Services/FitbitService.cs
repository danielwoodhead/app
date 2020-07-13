using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Services;
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
        private readonly FitbitSettings _fitBitSettings;

        public FitbitService(
            IEventPublisher eventPublisher,
            IOperationContext operationContext,
            IFitbitClient fitbitClient,
            IIntegrationsService integrationsService,
            IOptions<FitbitSettings> fitBitSettings)
        {
            _eventPublisher = eventPublisher;
            _operationContext = operationContext;
            _fitbitClient = fitbitClient;
            _integrationsService = integrationsService;
            _fitBitSettings = fitBitSettings.Value;
        }

        public async Task CreateIntegrationAsync(string code)
        {
            TokenResponse tokenResponse = await _fitbitClient.AuthenticateAsync(code);

            if (tokenResponse.IsError)
                throw new Exception(tokenResponse.Error); // TODO: if invalid code - return 4XX else throw exception

            await _integrationsService.CreateIntegrationAsync(
                new CreateIntegrationRequest
                {
                    Provider = Provider.Fitbit
                });
        }

        public async Task ProcessUpdateNotificationAsync(IEnumerable<FitbitUpdateNotification> request)
        {
            // TODO: signature verification

            await _eventPublisher.PublishAsync(request.Select(update =>
                new IntegrationProviderUpdateEvent(
                    id: Guid.NewGuid().ToString(),
                    subject: update.SubscriptionId,
                    eventTime: DateTime.UtcNow,
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
