using System.Net.Http;
using System.Threading.Tasks;
using MyHealth.App.Api.Core.Http;
using MyHealth.App.Api.Integrations.Models;

namespace MyHealth.App.Api.Integrations.Clients
{
    public class IntegrationsClient : IIntegrationsClient
    {
        private readonly HttpClient _httpClient;

        public IntegrationsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateFitbitIntegrationAsync(CreateFitbitIntegrationRequest request)
        {
            return await _httpClient.SendAsync(HttpMethod.Post, Endpoints.FitbitIntegrations, request);
        }

        public async Task<HttpResponseMessage> DeleteIntegrationAsync(string id)
        {
            return await _httpClient.SendAsync(HttpMethod.Delete, Endpoints.Integration(id));
        }

        public async Task<HttpResponseMessage> GetIntegrationAsync(string id)
        {
            return await _httpClient.SendAsync(HttpMethod.Get, Endpoints.Integration(id));
        }

        public async Task<HttpResponseMessage> SearchIntegrationsAsync()
        {
            return await _httpClient.SendAsync(HttpMethod.Get, Endpoints.SearchIntegrations);
        }

        private class Endpoints
        {
            public const string FitbitIntegrations = "integrations/fitbit";
            public const string SearchIntegrations = "integrations";
            public static string Integration(string id) => $"integrations/{id}";
        }
    }
}
