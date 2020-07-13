using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyHealth.Web.Core.Integrations
{
    public interface IIntegrationsClient
    {
        Task<SearchIntegrationsResponse> GetAllIntegrationsAsync();
        Task CreateFitbitIntegrationAsync(string code);
    }

    public class IntegrationsClient : IIntegrationsClient
    {
        private readonly HttpClient _httpClient;

        public IntegrationsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateFitbitIntegrationAsync(string code)
        {
            await _httpClient.PostAsJsonAsync(
                "integrations/fitbit",
                new AuthorizationCodeRequest { Code = code });
        }

        public async Task<SearchIntegrationsResponse> GetAllIntegrationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<SearchIntegrationsResponse>("integrations");
        }
    }

    public class SearchIntegrationsResponse
    {
        public IEnumerable<Integration> Integrations { get; set; }
    }

    public class Integration
    {
        public string Provider { get; set; }
    }

    public class AuthorizationCodeRequest
    {
        public string Code { get; set; }
    }
}
