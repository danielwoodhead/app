using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Clients
{
    public class StravaClient : IStravaClient
    {
        private readonly HttpClient _httpClient;
        private readonly StravaSettings _stravaSettings;

        public StravaClient(HttpClient httpClient, IOptions<StravaSettings> stravaSettings)
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

        public async Task<StravaSubscription> CreateSubscriptionAsync(string callbackUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "push_subscriptions");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", _stravaSettings.ClientId },
                { "client_secret", _stravaSettings.ClientSecret },
                { "callback_url", callbackUrl },
                { "verify_token", _stravaSettings.SubscriptionVerifyToken }
            });

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<StravaSubscription>();
        }

        public async Task DeleteSubscriptionAsync(int subscriptionId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"push_subscriptions/{subscriptionId}");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", _stravaSettings.ClientId },
                { "client_secret", _stravaSettings.ClientSecret }
            });

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return;

            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<StravaSubscription>> GetSubscriptionsAsync()
        {
            string requestUri = QueryHelpers.AddQueryString(
                "push_subscriptions",
                new Dictionary<string, string>
                {
                    { "client_id", _stravaSettings.ClientId },
                    { "client_secret", _stravaSettings.ClientSecret }
                });

            return await _httpClient.GetFromJsonAsync<IEnumerable<StravaSubscription>>(requestUri);
        }

        public Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
