using System.Net.Http;

namespace MyHealth.App.Api.HealthRecord.Clients
{
    public class HealthRecordClient : IHealthRecordClient
    {
        private readonly HttpClient _httpClient;

        public HealthRecordClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
