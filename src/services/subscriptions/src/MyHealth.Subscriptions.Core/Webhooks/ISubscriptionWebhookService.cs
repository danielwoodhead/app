using System.Threading.Tasks;
using MyHealth.Subscriptions.Models;
using MyHealth.Subscriptions.Models.Requests;

namespace MyHealth.Subscriptions.Core.Webhooks
{
    public interface ISubscriptionWebhookService
    {
        Task<OperationResult<SubscriptionWebhook>> AddSubscriptionWebhookAsync(CreateSubscriptionWebhookRequestModel request);
    }
}
