using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.HealthRecord.Models;

namespace MyHealth.HealthRecord.Core.Data
{
    public interface IObservationsRepository
    {
        Task<IEnumerable<Observation>> GetObservationsAsync(string userId);
    }
}
