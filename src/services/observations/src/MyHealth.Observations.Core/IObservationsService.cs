using System.Threading.Tasks;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Requests;
using MyHealth.Observations.Models.Responses;

namespace MyHealth.Observations.Core
{
    public interface IObservationsService
    {
        Task<Observation> CreateObservationAsync(CreateObservationRequest request);
        Task DeleteObservationAsync(string id);
        Task<Observation> GetObservationAsync(string id);
        Task<SearchObservationsResponse> SearchObservationsAsync();
        Task<Observation> UpdateObservationAsync(string id, UpdateObservationRequest request);
    }
}
