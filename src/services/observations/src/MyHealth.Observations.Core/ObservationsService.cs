using System.Threading.Tasks;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Requests;
using MyHealth.Observations.Models.Responses;

namespace MyHealth.Observations.Core
{
    public class ObservationsService : IObservationsService
    {
        public Task<Observation> CreateObservationAsync(CreateObservationRequest request)
        {
            throw new System.NotImplementedException();
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
