using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Mobile.Core.Models.Observations;

namespace MyHealth.Mobile.Core.Services.Observations
{
    public interface IObservationsService
    {
        Task AddObservationAsync(Observation observation);
        Task DeleteObservationAsync(string id);
        Task<Observation> GetObservationAsync(string id);
        Task<IEnumerable<Observation>> GetObservationsAsync();
        Task UpdateObservationAsync(Observation observation);
    }
}
