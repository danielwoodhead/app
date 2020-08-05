using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MyHealth.Integrations.Fitbit.Models;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public class FitbitClient : IFitbitClient
    {
        private readonly HttpClient _httpClient;

        public FitbitClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<AddFitbitSubscriptionResponse> AddSubscriptionAsync(string subscriptionId, string accessToken, string collectionPath = null, string subscriberId = null)
        {
            string url = BuildAddSubscriptionUrl(subscriptionId, collectionPath);
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (subscriberId != null)
                request.Headers.Add(FitbitConstants.SubscriberIdHeader, subscriberId);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AddFitbitSubscriptionResponse>();
        }

        public async Task DeleteSubscriptionAsync(string subscriptionId, string collectionPath = null, string subscriberId = null)
        {
            string url = BuildAddSubscriptionUrl(subscriptionId, collectionPath);
            var request = new HttpRequestMessage(HttpMethod.Delete, url);

            if (subscriberId != null)
                request.Headers.Add(FitbitConstants.SubscriberIdHeader, subscriberId);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private static string BuildAddSubscriptionUrl(string subscriptionId, string collectionPath)
        {
            string url = "1/user/-";

            if (!string.IsNullOrEmpty(collectionPath))
                url += $"/{collectionPath}";

            url += $"/apiSubscriptions/{subscriptionId}.json";

            return url;
        }
    }
}
