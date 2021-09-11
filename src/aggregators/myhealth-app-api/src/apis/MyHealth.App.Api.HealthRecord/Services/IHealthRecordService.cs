using System.Threading.Tasks;
using MyHealth.App.Api.HealthRecord.Models;

namespace MyHealth.App.Api.HealthRecord.Services
{
    public interface IHealthRecordService
    {
        Task<SearchObservationsResponse> GetObservationsAsync();
    }
}
