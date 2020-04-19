using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Repository;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using MyHealth.Integrations.Models.Requests;
using MyHealth.Integrations.Models.Response;
using MyHealth.Integrations.Utility;

namespace MyHealth.Integrations.Core
{
    public class IntegrationsService : IIntegrationsService
    {
        private const string EventDataVersion = "1.0";
        private const string EventSourceSystem = "myhealth:integrations:api";

        private readonly IIntegrationsRepository _integrationsRepository;
        private readonly IOperationContext _operationContext;
        private readonly IEventPublisher _eventPublisher;

        public IntegrationsService(
            IIntegrationsRepository integrationsRepository,
            IOperationContext operationContext,
            IEventPublisher eventPublisher)
        {
            _integrationsRepository = integrationsRepository;
            _operationContext = operationContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<Integration> CreateIntegrationAsync(CreateIntegrationRequest request)
        {
            Integration integration = await _integrationsRepository.CreateIntegrationAsync(request, _operationContext.UserId);
            await _eventPublisher.PublishAsync(CreateIntegrationCreatedEvent(integration));

            return integration;
        }

        public async Task DeleteIntegrationAsync(string id)
        {
            await _integrationsRepository.DeleteIntegrationAsync(id, _operationContext.UserId);

            await _eventPublisher.PublishAsync(CreateIntegrationDeletedEvent(id));
        }

        public async Task<Integration> GetIntegrationAsync(string id)
        {
            return await _integrationsRepository.GetIntegrationAsync(id, _operationContext.UserId);
        }

        public async Task<SearchIntegrationsResponse> SearchIntegrationsAsync()
        {
            IEnumerable<Integration> integrations = await _integrationsRepository.GetIntegrationsAsync(_operationContext.UserId);

            return new SearchIntegrationsResponse
            {
                Integrations = integrations
            };
        }

        public async Task<Integration> UpdateIntegrationAsync(string id, UpdateIntegrationRequest request)
        {
            Integration integration = await _integrationsRepository.UpdateIntegrationAsync(id, _operationContext.UserId, request);
            await _eventPublisher.PublishAsync(CreateIntegrationUpdatedEvent(integration));

            return integration;
        }

        private IntegrationCreatedEvent CreateIntegrationCreatedEvent(Integration integration) =>
            new IntegrationCreatedEvent(
                id: Guid.NewGuid().ToString(),
                subject: integration.Id,
                eventTime: DateTime.UtcNow,
                dataVersion: EventDataVersion,
                data: CreateEventData());

        private IntegrationDeletedEvent CreateIntegrationDeletedEvent(string id) =>
            new IntegrationDeletedEvent(
                id: Guid.NewGuid().ToString(),
                subject: id,
                eventTime: DateTime.UtcNow,
                dataVersion: EventDataVersion,
                data: CreateEventData());

        private IntegrationUpdatedEvent CreateIntegrationUpdatedEvent(Integration integration) =>
            new IntegrationUpdatedEvent(
                id: Guid.NewGuid().ToString(),
                subject: integration.Id,
                eventTime: DateTime.UtcNow,
                dataVersion: EventDataVersion,
                data: CreateEventData());

        private IntegrationEventData CreateEventData() =>
            new IntegrationEventData
            {
                OperationId = _operationContext.OperationId,
                SourceSystem = EventSourceSystem,
                SubjectSystem = EventSourceSystem,
                UserId = _operationContext.UserId
            };
    }
}
