using System.Threading.Tasks;
using MyHealth.Observations.Core.Events;
using MyHealth.Observations.Core.Repository;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Requests;
using MyHealth.Observations.Models.Responses;

namespace MyHealth.Observations.Core
{
    public class ObservationsService : IObservationsService
    {
        private readonly IObservationsRepository _repository;
        private readonly IEventPublisher _eventPublisher;

        public ObservationsService(
            IObservationsRepository repository,
            IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        public async Task<Observation> CreateObservationAsync(CreateObservationRequest request)
        {
            Observation observation = await _repository.CreateObservationAsync(request);
            await _eventPublisher.PublishObservationCreatedEvent(observation);

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
    }
}
