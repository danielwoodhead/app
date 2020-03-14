using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Observations.Core.Events;
using MyHealth.Observations.Core.Repository;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Events;
using MyHealth.Observations.Models.Requests;
using MyHealth.Observations.Models.Responses;
using MyHealth.Observations.Utility;

namespace MyHealth.Observations.Core
{
    public class ObservationsService : IObservationsService
    {
        private const string EventDataVersion = "1.0";
        private const string EventSourceSystem = "myhealth:observations:api";
        private readonly IObservationsRepository _repository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IOperationContext _operationContext;

        public ObservationsService(
            IObservationsRepository repository,
            IEventPublisher eventPublisher,
            IOperationContext operationContext)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _operationContext = operationContext;
        }

        public async Task<Observation> CreateObservationAsync(CreateObservationRequest request)
        {
            Observation observation = await _repository.CreateObservationAsync(request, _operationContext.UserId);
            await _eventPublisher.PublishAsync(CreateObservationCreatedEvent(observation));

            return observation;
        }

        public async Task DeleteObservationAsync(string id)
        {
            await _repository.DeleteObservationAsync(id);
            await _eventPublisher.PublishAsync(CreateObservationDeletedEvent(id));
        }

        public async Task<Observation> GetObservationAsync(string id)
        {
            return await _repository.GetObservationAsync(id);
        }

        public async Task<SearchObservationsResponse> SearchObservationsAsync()
        {
            IEnumerable<Observation> observations = await _repository.GetObservationsAsync(_operationContext.UserId);

            return new SearchObservationsResponse
            {
                Observations = observations
            };
        }

        public async Task<Observation> UpdateObservationAsync(string id, UpdateObservationRequest request)
        {
            Observation observation = await _repository.UpdateObservationAsync(id, request);
            await _eventPublisher.PublishAsync(CreateObservationUpdatedEvent(observation));

            return observation;
        }

        private ObservationCreatedEvent CreateObservationCreatedEvent(Observation observation) =>
            new ObservationCreatedEvent(
                id: Guid.NewGuid().ToString(),
                subject: observation.Id,
                eventTime: DateTime.UtcNow,
                dataVersion: EventDataVersion,
                data: CreateEventData());

        private ObservationDeletedEvent CreateObservationDeletedEvent(string observationId) =>
            new ObservationDeletedEvent(
                id: Guid.NewGuid().ToString(),
                subject: observationId,
                eventTime: DateTime.UtcNow,
                dataVersion: EventDataVersion,
                data: CreateEventData());

        private ObservationUpdatedEvent CreateObservationUpdatedEvent(Observation observation) =>
            new ObservationUpdatedEvent(
                id: Guid.NewGuid().ToString(),
                subject: observation.Id,
                eventTime: DateTime.UtcNow,
                dataVersion: EventDataVersion,
                data: CreateEventData());

        private ObservationEventData CreateEventData() =>
            new ObservationEventData
            {
                OperationId = _operationContext.OperationId,
                SourceSystem = EventSourceSystem,
                SubjectSystem = EventSourceSystem,
                UserId = _operationContext.UserId
            };
    }
}
