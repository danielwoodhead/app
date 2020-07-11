using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MyHealth.Integrations.Fitbit.Models;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public class FitbitClient : IFitbitClient
    {
        private const string SubscriberIdHeader = "X-Fitbit-Subscriber-Id";

        private readonly HttpClient _httpClient;

        public FitbitClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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
