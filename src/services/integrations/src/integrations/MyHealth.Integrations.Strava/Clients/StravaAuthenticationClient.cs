using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Clients
{
    public class StravaAuthenticationClient : IStravaAuthenticationClient
    {
        private readonly HttpClient _httpClient;
        private readonly StravaSettings _stravaSettings;

        public StravaAuthenticationClient(HttpClient httpClient, IOptions<StravaSettings> stravaSettings)
        {
            _httpClient = httpClient;
            _stravaSettings = stravaSettings.Value;
        }

        public async Task<TokenResponse> AuthenticateAsync(string code)
        {
            string requestUri = QueryHelpers.AddQueryString(
                "oauth/token",
                new Dictionary<string, string>
                {
                    { "client_id", _stravaSettings.ClientId },
                    { "client_secret", _stravaSettings.ClientSecret },
                    { "code", code },
                    { "grant_type", "authorization_code" }
                });

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<TokenResponse>();
        }

        public Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
