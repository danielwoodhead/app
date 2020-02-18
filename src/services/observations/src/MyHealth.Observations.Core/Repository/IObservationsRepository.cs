using System.Threading.Tasks;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Requests;

namespace MyHealth.Observations.Core.Repository
{
    public interface IObservationsRepository
    {
        Task<Observation> CreateObservationAsync(CreateObservationRequest request);
        Task DeleteObservationAsync(string id);
        Task<Observation> GetObservationAsync(string id);
        Task<Observation> UpdateObservationAsync(string id, UpdateObservationRequest request);
    }
}
