using System.Threading.Tasks;
using MyHealth.HealthRecord.Models;
using MyHealth.HealthRecord.Models.Requests;
using MyHealth.HealthRecord.Models.Responses;

namespace MyHealth.HealthRecord.Core
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
