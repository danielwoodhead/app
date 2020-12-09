using System.Threading.Tasks;

namespace MyHealth.Subscriptions.Core.Webhooks
{
    public interface ISubscriptionWebhookClient
    {
        Task<SubscriptionWebhookValidationResponse> ValidateAsync(string url, string verificationCode);
    }
}
