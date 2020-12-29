using System.Threading.Tasks;
using MyHealth.Web.App.Areas.HealthRecord.Models;

namespace MyHealth.Web.App.Areas.HealthRecord.Client
{
    public interface IHealthRecordClient
    {
        Task<SearchObservationsResponse> GetObservationsAsync();
    }
}
