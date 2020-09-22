using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using MyHealth.Web.Core.Models;

namespace MyHealth.Web.Core.AppApi
{
    public class AppApiClient : IAppApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AppApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        #region HealthRecord

        public async Task<SearchObservationsResponse> GetObservationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<SearchObservationsResponse>("observations");
        }

        #endregion HealthRecord

        #region Integrations

        public async Task DeleteIntegrationAsync(string id)
        {
            await _httpClient.DeleteAsync($"integrations/{id}");
        }

        public async Task<GetIntegrationsResponse> GetAllIntegrationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<GetIntegrationsResponse>("integrations", _jsonSerializerOptions);
        }

        public async Task CreateFitbitIntegrationAsync(string code, string redirectUri)
        {
            await _httpClient.PostAsJsonAsync(
                "integrations/fitbit",
                new CreateFitbitIntegrationRequest
                {
                    Code = code,
                    RedirectUri = new Uri(redirectUri)
                },
                _jsonSerializerOptions);
        }

        public async Task<string> GetFitbitAuthenticationUriAsync(string redirectUri)
        {
            return await _httpClient.GetStringAsync(
                QueryHelpers.AddQueryString(
                    "integrations/fitbit/authenticationUri",
                    "redirectUri",
                    redirectUri));
        }

        public async Task CreateStravaIntegrationAsync(string code)
        {
            await _httpClient.PostAsJsonAsync(
                "integrations/strava",
                new CreateStravaIntegrationRequest
                {
                    Code = code
                },
                _jsonSerializerOptions);
        }

        public async Task<string> GetStravaAuthenticationUriAsync(string redirectUri)
        {
            return await _httpClient.GetStringAsync(
                QueryHelpers.AddQueryString(
                    "integrations/strava/authenticationUri",
                    "redirectUri",
                    redirectUri));
        }

        #endregion
    }
}
