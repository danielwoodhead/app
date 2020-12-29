using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyHealth.Web.App.Areas.HealthRecord.Models;

namespace MyHealth.Web.App.Areas.HealthRecord.Client
{
    public class HealthRecordClient : IHealthRecordClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HealthRecordClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task<SearchObservationsResponse> GetObservationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<SearchObservationsResponse>("observations");
        }
    }
}
