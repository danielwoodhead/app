using System.Net.Http;
using System.Threading.Tasks;
using MyHealth.App.Api.Core.Http;

namespace MyHealth.App.Api.HealthRecord.Clients
{
    public class HealthRecordClient : IHealthRecordClient
    {
        private readonly HttpClient _httpClient;

        public HealthRecordClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetObservationsAsync()
        {
            return await _httpClient.SendAsync(HttpMethod.Get, Endpoints.Observations);
        }

        private class Endpoints
        {
            public const string Observations = "observations";
        }
    }
}
