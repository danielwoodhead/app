using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.AspNetCore.Context;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Core.Events;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using MyHealth.Integrations.Strava.Clients.Models;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public class StravaUpdateService : IStravaUpdateService
    {
        private readonly IStravaSubscriptionService _stravaSubscriptionService;
        private readonly IIntegrationRepository _integrationRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IOperationContext _operationContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<StravaIntegrationService> _logger;

        public StravaUpdateService(
            IStravaSubscriptionService stravaSubscriptionService,
            IIntegrationRepository integrationRepository,
            IEventPublisher eventPublisher,
            IOperationContext operationContext,
            IDateTimeProvider dateTimeProvider,
            ILogger<StravaIntegrationService> logger)
        {
            _stravaSubscriptionService = stravaSubscriptionService;
            _integrationRepository = integrationRepository;
            _eventPublisher = eventPublisher;
            _operationContext = operationContext;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task ProcessUpdateNotification(StravaUpdateNotification updateNotification)
        {
            StravaSubscription subscription = await _stravaSubscriptionService.GetSubscriptionAsync();

            if (updateNotification.SubscriptionId != subscription.Id)
            {
                _logger.LogWarning($"Received Strava update for unrecognised subscription ID: {updateNotification.SubscriptionId}.");
                return;
            }

            Integration integration = await _integrationRepository.GetIntegrationAsync(Provider.Strava, updateNotification.OwnerId.ToString());

            if (integration is null)
            {
                _logger.LogWarning($"Received Strava update for owner ID with no integration '{updateNotification.OwnerId}'.");
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
                        Provider = Provider.Strava,
                        ProviderData = updateNotification,
                        UserId = integration.UserId
                    }));
        }
    }
}
