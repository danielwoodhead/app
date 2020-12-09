using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using MyHealth.Extensions.Azure.Storage.Table;
using MyHealth.Subscriptions.Core.Webhooks;
using MyHealth.Subscriptions.Models;
using MyHealth.Subscriptions.TableStorage.Entities;

namespace MyHealth.Subscriptions.TableStorage
{
    public class TableStorageSubscriptionWebhookRepository : ISubscriptionWebhookRepository
    {
        private const string TableName = "SubscriptionWebhooks";

        private readonly CloudTableClient _cloudTableClient;

        public TableStorageSubscriptionWebhookRepository(CloudTableClient cloudTableClient)
        {
            _cloudTableClient = cloudTableClient;
        }

        private CloudTable GetTable() => _cloudTableClient.GetTableReference(TableName);

        public async Task<SubscriptionWebhook> AddSubscriptionWebhookAsync(string webhookUrl, string clientId)
        {
            var entity = new SubscriptionWebhookEntity(
                id: Guid.NewGuid().ToString(),
                webhookUrl: webhookUrl,
                clientId: clientId);

            await GetTable().InsertAsync(entity);

            return new SubscriptionWebhook
            {
                Id = entity.Id,
                WebhookUrl = entity.WebhookUrl,
                ClientId = entity.ClientId
            };
        }

        public async Task<SubscriptionWebhook> GetSubscriptionWebhookAsync(string clientId)
        {
            var entity = (await GetTable().RetrievePartitionAsync<SubscriptionWebhookEntity>(clientId)).FirstOrDefault();

            if (entity is null)
                return null;

            return new SubscriptionWebhook
            {
                Id = entity.Id,
                WebhookUrl = entity.WebhookUrl,
                ClientId = entity.ClientId
            };
        }
    }
}
