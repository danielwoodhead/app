using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Core.Events;
using MyHealth.Integrations.Core.Extensions;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using MyHealth.Integrations.Models.Response;

namespace MyHealth.Integrations.Core.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly IIntegrationRepository _integrationRepository;
        private readonly IEnumerable<IIntegrationSystemService> _integrationSystemServices;
        private readonly IUserOperationContext _operationContext;
        private readonly IEventPublisher _eventPublisher;

        public IntegrationService(
            IIntegrationRepository integrationRepository,
            IEnumerable<IIntegrationSystemService> integrationSystemServices,
            IUserOperationContext operationContext,
            IEventPublisher eventPublisher)
        {
            _integrationRepository = integrationRepository;
            _integrationSystemServices = integrationSystemServices;
            _operationContext = operationContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<Integration> CreateIntegrationAsync(ProviderRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            IIntegrationSystemService integrationSystemService = _integrationSystemServices.GetService(request.Provider);
            ProviderResult creationResult = await integrationSystemService.CreateIntegrationAsync(request);

            Integration integration = await _integrationRepository.CreateIntegrationAsync(_operationContext.UserId, creationResult.Provider, creationResult.Data);
            await _eventPublisher.PublishAsync(CreateIntegrationCreatedEvent(integration));

            return integration;
        }

        public async Task DeleteIntegrationAsync(string id)
        {
            Integration integration = await _integrationRepository.GetIntegrationAsync(id, _operationContext.UserId);

            if (integration != null)
            {
                IIntegrationSystemService integrationSystemService = _integrationSystemServices.GetService(integration.Provider);
                await integrationSystemService.DeleteIntegrationAsync(_operationContext.UserId);
                await _integrationRepository.DeleteIntegrationAsync(id, _operationContext.UserId);
                await _eventPublisher.PublishAsync(CreateIntegrationDeletedEvent(integration));
            }
        }

        public async Task<Integration> GetIntegrationAsync(string id)
        {
            return await _integrationRepository.GetIntegrationAsync(id, _operationContext.UserId);
        }

        public async Task<Integration> GetIntegrationAsync(string userId, Provider provider)
        {
            return await _integrationRepository.GetIntegrationAsync(userId, provider);
        }

        public async Task<SearchIntegrationsResponse> SearchIntegrationsAsync()
        {
            IEnumerable<Integration> integrations = await _integrationRepository.GetIntegrationsAsync(_operationContext.UserId);

            return new SearchIntegrationsResponse
            {
                Integrations = integrations
            };
        }

        public async Task UpdateIntegrationAsync(string userId, Provider provider, object integrationData)
        {
            await _integrationRepository.UpdateIntegrationAsync(userId, provider, integrationData);
        }

        private IntegrationCreatedEvent CreateIntegrationCreatedEvent(Integration integration) =>
            new IntegrationCreatedEvent(
                id: Guid.NewGuid().ToString(),
                subject: integration.Id,
                eventTime: DateTime.UtcNow,
                dataVersion: EventConstants.EventDataVersion,
                data: CreateEventData(integration));

        private IntegrationDeletedEvent CreateIntegrationDeletedEvent(Integration integration) =>
            new IntegrationDeletedEvent(
                id: Guid.NewGuid().ToString(),
                subject: integration.Id,
                eventTime: DateTime.UtcNow,
                dataVersion: EventConstants.EventDataVersion,
                data: CreateEventData(integration));

        private IntegrationEventData CreateEventData(Integration integration) =>
            new IntegrationEventData
            {
                OperationId = _operationContext.OperationId,
                SourceSystem = EventConstants.IntegrationsApi,
                SubjectSystem = EventConstants.MyHealth,
                Provider = integration.Provider,
                UserId = _operationContext.UserId
            };
    }
}
