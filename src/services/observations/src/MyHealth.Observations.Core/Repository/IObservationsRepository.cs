using System.Threading.Tasks;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Requests;

namespace MyHealth.Observations.Core.Repository
{
    public interface IObservationsRepository
    {
        Task<Observation> CreateObservationAsync(CreateObservationRequest request);
    }
}
