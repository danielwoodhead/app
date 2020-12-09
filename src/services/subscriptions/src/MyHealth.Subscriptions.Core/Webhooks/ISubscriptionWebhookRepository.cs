using System.Threading.Tasks;
using MyHealth.Subscriptions.Models;

namespace MyHealth.Subscriptions.Core.Webhooks
{
    public interface ISubscriptionWebhookRepository
    {
        Task<SubscriptionWebhook> AddSubscriptionWebhookAsync(string webhookUrl, string clientId);
        Task<SubscriptionWebhook> GetSubscriptionWebhookAsync(string clientId);
    }
}
