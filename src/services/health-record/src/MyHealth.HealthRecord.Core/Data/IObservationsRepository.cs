using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.HealthRecord.Models;
using MyHealth.HealthRecord.Models.Requests;

namespace MyHealth.HealthRecord.Core.Data
{
    public interface IObservationsRepository
    {
        Task<Observation> CreateObservationAsync(CreateObservationRequest request, string userId);
        Task DeleteObservationAsync(string id);
        Task<Observation> GetObservationAsync(string id);
        Task<IEnumerable<Observation>> GetObservationsAsync(string userId);
        Task<Observation> UpdateObservationAsync(string id, UpdateObservationRequest request);
    }
}
