using Microsoft.Azure.Cosmos.Table;

namespace MyHealth.Subscriptions.TableStorage.Entities
{
    public class SubscriptionWebhookEntity : TableEntity
    {
        public string Id { get; set; }
        public string WebhookUrl { get; set; }
        public string ClientId { get; set; }

        public SubscriptionWebhookEntity()
        {
        }

        public SubscriptionWebhookEntity(string id, string webhookUrl, string clientId)
        {
            RowKey = Id = id;
            WebhookUrl = webhookUrl;
            PartitionKey = ClientId = clientId;
        }
    }
}
