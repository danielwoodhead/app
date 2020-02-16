using System;
using System.Threading.Tasks;
using MyHealth.Observations.Core.Events;
using MyHealth.Observations.Core.Repository;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Events;
using MyHealth.Observations.Models.Events.Base;
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
            Observation observation = await _repository.CreateObservationAsync(request);
            await _eventPublisher.PublishAsync(CreateObservationCreatedEvent(observation));

            return observation;
        }

        public Task DeleteObservationAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Observation> GetObservationAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SearchObservationsResponse> SearchObservationsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Observation> UpdateObservationAsync(string id, UpdateObservationRequest request)
        {
            throw new System.NotImplementedException();
        }

        private ObservationCreatedEvent CreateObservationCreatedEvent(Observation observation) =>
            new ObservationCreatedEvent(
                id: Guid.NewGuid().ToString(),
                subject: observation.Id,
                eventTime: DateTime.UtcNow,
                dataVersion: EventDataVersion,
                data: new EventData
                {
                    OperationId = _operationContext.OperationId,
                    SourceSystem = EventSourceSystem,
                    SubjectSystem = EventSourceSystem
                });
    }
}
