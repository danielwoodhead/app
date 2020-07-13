using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Fitbit.Models;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public class FitbitClient : IFitbitClient
    {
        private const string SubscriberIdHeader = "X-Fitbit-Subscriber-Id";

        private readonly HttpClient _httpClient;
        private readonly FitbitSettings _fitbitSettings;

        public FitbitClient(HttpClient httpClient, IOptions<FitbitSettings> fitbitSettings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _fitbitSettings = fitbitSettings?.Value ?? throw new ArgumentNullException(nameof(fitbitSettings));
        }

        public async Task<AddFitbitSubscriptionResponse> AddSubscriptionAsync(string subscriptionId, string collectionPath = null, string subscriberId = null)
        {
            string url = BuildAddSubscriptionUrl(subscriptionId, collectionPath);
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (subscriberId != null)
                request.Headers.Add(SubscriberIdHeader, subscriberId);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<AddFitbitSubscriptionResponse>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<TokenResponse> AuthenticateAsync(string code)
        {
            return await _httpClient.RequestAuthorizationCodeTokenAsync(
                new AuthorizationCodeTokenRequest
                {
                    Address = _httpClient.BaseAddress.AbsoluteUri + "/oauth2/token",
                    ClientId = _fitbitSettings.ClientId,
                    RedirectUri = "", // TODO: should be passed in
                    Code = code
                });
        }

        private static string BuildAddSubscriptionUrl(string subscriptionId, string collectionPath)
        {
            string url = "user/-";

            if (!string.IsNullOrEmpty(collectionPath))
                url += $"/{collectionPath}";

            url += $"/apiSubscriptions/{subscriptionId}.json";

            return url;
        }
    }
}
