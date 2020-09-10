using System.Net.Http;
using System.Threading.Tasks;

namespace MyHealth.App.Api.HealthRecord.Clients
{
    public interface IHealthRecordClient
    {
        Task<HttpResponseMessage> GetObservationsAsync();
    }
}
