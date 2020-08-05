using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyHealth.Web.Core.Integrations
{
    public interface IIntegrationsClient
    {
        Task CreateFitbitIntegrationAsync(string code, string redirectUri);
        Task DeleteIntegrationAsync(string id);
        Task<SearchIntegrationsResponse> GetAllIntegrationsAsync();
    }

    public class IntegrationsClient : IIntegrationsClient
    {
        private readonly HttpClient _httpClient;

        public IntegrationsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateFitbitIntegrationAsync(string code, string redirectUri)
        {
            await _httpClient.PostAsJsonAsync(
                "integrations/fitbit",
                new AuthorizationCodeRequest
                {
                    Code = code,
                    RedirectUri = redirectUri
                });
        }

        public async Task DeleteIntegrationAsync(string id)
        {
            await _httpClient.DeleteAsync($"integrations/{id}");
        }

        public async Task<SearchIntegrationsResponse> GetAllIntegrationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<SearchIntegrationsResponse>("integrations");
        }
    }

    public class SearchIntegrationsResponse
    {
        public IEnumerable<Integration> Integrations { get; set; } = Enumerable.Empty<Integration>();
    }

    public class Integration
    {
        public string Id { get; set; }
        public string Provider { get; set; }
    }

    public class AuthorizationCodeRequest
    {
        public string Code { get; set; }
        public string RedirectUri { get; set; }
    }
}
