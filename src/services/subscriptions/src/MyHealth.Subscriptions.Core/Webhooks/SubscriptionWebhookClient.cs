using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace MyHealth.Subscriptions.Core.Webhooks
{
    public class SubscriptionWebhookClient : ISubscriptionWebhookClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SubscriptionWebhookClient> _logger;

        public SubscriptionWebhookClient(HttpClient httpClient, ILogger<SubscriptionWebhookClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<SubscriptionWebhookValidationResponse> ValidateAsync(string url, string verificationCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString(url, "verify", verificationCode));

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred calling subscription webhook");
                return null;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<SubscriptionWebhookValidationResponse>();
        }
    }
}
