using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyHealth.Mobile.Core.Services.Observations
{
    public interface IObservationsService
    {
        Task AddObservationAsync(string observation);
        Task<IEnumerable<string>> GetObservationsAsync();
    }
}
