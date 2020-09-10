using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using MyHealth.Web.Core.Models;

namespace MyHealth.Web.Core.AppApi
{
    public class AppApiClient : IAppApiClient
    {
        private readonly HttpClient _httpClient;

        public AppApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region HealthRecord

        public async Task<SearchObservationsResponse> GetObservationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<SearchObservationsResponse>("observations");
        }

        #endregion HealthRecord

        #region Integrations

        public async Task CreateFitbitIntegrationAsync(string code, string redirectUri)
        {
            await _httpClient.PostAsJsonAsync(
                "integrations/fitbit",
                new AuthorizationCodeRequest
                {
                    Code = code,
                    RedirectUri = new Uri(redirectUri)
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

        public async Task<string> GetFitbitAuthenticationUriAsync(string redirectUri)
        {
            return await _httpClient.GetStringAsync(
                QueryHelpers.AddQueryString(
                    "integrations/fitbit/authenticationUri",
                    "redirectUri",
                    redirectUri));
        }

        #endregion
    }
}
