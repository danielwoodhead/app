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

        public async Task<HttpResponseMessage> GetIntegrationsAsync()
        {
            return await _httpClient.SendAsync(HttpMethod.Get, Endpoints.Integrations);
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

        #region Strava

        public async Task<HttpResponseMessage> CreateStravaIntegrationAsync(CreateStravaIntegrationRequest request)
        {
            return await _httpClient.SendAsync(HttpMethod.Post, Endpoints.StravaIntegrations, request);
        }

        public async Task<HttpResponseMessage> GetStravaAuthenticationUri(string redirectUri)
        {
            return await _httpClient.SendAsync(HttpMethod.Get, Endpoints.StravaAuthenticationUri(redirectUri));
        }

        #endregion

        private class Endpoints
        {
            public const string Integrations = "integrations";
            public static string Integration(string id) => $"integrations/{id}";

            #region Fitbit

            public const string FitbitIntegrations = "integrations/fitbit";
            public static string FitbitAuthenticationUri(string redirectUri) => QueryHelpers.AddQueryString("integrations/fitbit/authenticationUri", "redirectUri", redirectUri);

            #endregion Fitbit

            #region Strava

            public const string StravaIntegrations = "integrations/strava";
            public static string StravaAuthenticationUri(string redirectUri) => QueryHelpers.AddQueryString("integrations/strava/authenticationUri", "redirectUri", redirectUri);

            #endregion
        }
    }
}
