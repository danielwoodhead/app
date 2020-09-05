using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
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

        #region Fitbit

        public async Task<HttpResponseMessage> CreateFitbitIntegrationAsync(CreateFitbitIntegrationRequest request)
        {
            return await _httpClient.SendAsync(HttpMethod.Post, Endpoints.FitbitIntegrations, request);
        }

        public async Task<HttpResponseMessage> GetFitbitAuthenticationUri(string redirectUri)
        {
            return await _httpClient.SendAsync(HttpMethod.Get, Endpoints.FitbitAuthenticationUri(redirectUri));
        }

        #endregion Fitbit

        private class Endpoints
        {
            public const string SearchIntegrations = "integrations";
            public static string Integration(string id) => $"integrations/{id}";

            #region Fitbit

            public const string FitbitIntegrations = "integrations/fitbit";
            public static string FitbitAuthenticationUri(string redirectUri) => QueryHelpers.AddQueryString("integrations/fitbit/authenticationUri", "redirectUri", redirectUri);

            #endregion Fitbit
        }
    }
}
