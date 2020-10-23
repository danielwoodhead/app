using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Strava.Clients.Models;

namespace MyHealth.Integrations.Strava.Clients
{
    public class StravaClient : IStravaClient
    {
        private readonly HttpClient _httpClient;
        private readonly StravaSettings _stravaSettings;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public StravaClient(HttpClient httpClient, IOptions<StravaSettings> stravaSettings)
        {
            _httpClient = httpClient;
            _stravaSettings = stravaSettings.Value;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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

            return await response.Content.ReadFromJsonAsync<TokenResponse>(_jsonSerializerOptions);
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

            return await response.Content.ReadFromJsonAsync<StravaSubscription>(_jsonSerializerOptions);
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

        public async Task<DetailedActivity> GetActivityAsync(long activityId, string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"activities/{activityId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<DetailedActivity>(_jsonSerializerOptions);
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

            return await _httpClient.GetFromJsonAsync<IEnumerable<StravaSubscription>>(requestUri, _jsonSerializerOptions);
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            string requestUri = QueryHelpers.AddQueryString(
                "oauth/token",
                new Dictionary<string, string>
                {
                    { "client_id", _stravaSettings.ClientId },
                    { "client_secret", _stravaSettings.ClientSecret },
                    { "grant_type", "refresh_token" },
                    { "refresh_token", refreshToken },
                });

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<TokenResponse>(_jsonSerializerOptions);
        }
    }
}
