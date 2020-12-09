using System.Threading.Tasks;
using MyHealth.Subscriptions.Core.Webhooks;
using MyHealth.Subscriptions.Models;

namespace MyHealth.Subscriptions.Tests.Mocks
{
    public class MockSubscriptionWebhookRepository : ISubscriptionWebhookRepository
    {
        public SubscriptionWebhook GetSubscriptionReturnValue { get; set; }
        public SubscriptionWebhook AddSubscriptionReturnValue { get; set; }
        public Task<SubscriptionWebhook> AddSubscriptionWebhookAsync(string webhookUrl, string clientId) => Task.FromResult(AddSubscriptionReturnValue);
        public Task<SubscriptionWebhook> GetSubscriptionWebhookAsync(string clientId) => Task.FromResult(GetSubscriptionReturnValue);
    }
}
