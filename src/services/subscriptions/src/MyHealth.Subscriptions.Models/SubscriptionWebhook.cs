namespace MyHealth.Subscriptions.Models
{
    public class SubscriptionWebhook
    {
        public string Id { get; set; }
        public string WebhookUrl { get; set; }
        public string ClientId { get; set; }
    }
}
