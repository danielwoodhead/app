using System.Threading.Tasks;
using MyHealth.HealthRecord.Models.Responses;

namespace MyHealth.HealthRecord.Core
{
    public interface IObservationsService
    {
        Task<SearchObservationsResponse> SearchObservationsAsync();
    }
}
