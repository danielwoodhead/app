using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Data.Cosmos
{
    public class CosmosIntegrationRepository : IIntegrationRepository
    {
        private readonly CosmosClient _cosmosClient;

        public CosmosIntegrationRepository(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public async Task<Integration> CreateIntegrationAsync(string userId, Provider provider, string providerUserId, IProviderData data)
        {
            Container container = GetIntegrationsContainer();

            var entity = new IntegrationEntity
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Provider = provider,
                ProviderUserId = providerUserId,
                ProviderData = data
            };

            ItemResponse<IntegrationEntity> response = await container.CreateItemAsync(entity, new PartitionKey(entity.UserId));

            return response.Resource.Map();
        }

        public async Task DeleteIntegrationAsync(string id, string userId)
        {
            Container container = GetIntegrationsContainer();

            await container.DeleteItemAsync<IntegrationEntity>(id, new PartitionKey(userId));
        }

        public async Task<Integration> GetIntegrationAsync(string id, string userId)
        {
            var entities = await QueryIntegrationsAsync($"SELECT * FROM i WHERE i.id = '{id}'");

            var entity = entities.FirstOrDefault();

            if (entity is null)
                return null;

            return entity.Map();
        }

        public async Task<Integration> GetIntegrationAsync(string userId, Provider provider)
        {
            var entities = await QueryIntegrationsAsync($"SELECT * FROM i WHERE i.UserId = '{userId}' AND i.Provider = '{provider}'");

            var entity = entities.FirstOrDefault();

            if (entity is null)
                return null;

            return entity.Map();
        }

        public async Task<Integration> GetIntegrationAsync(Provider provider, string providerUserId)
        {
            var entities = await QueryIntegrationsAsync($"SELECT * FROM i WHERE i.Provider = '{provider}' AND i.ProviderUserId = '{providerUserId}'");

            var entity = entities.FirstOrDefault();

            if (entity is null)
                return null;

            return entity.Map();
        }

        public async Task<IEnumerable<Integration>> GetIntegrationsAsync(string userId)
        {
            var entities = await QueryIntegrationsAsync($"SELECT * FROM i WHERE i.UserId = '{userId}'");

            return entities.Select(e => e.Map());
        }

        public async Task UpdateIntegrationAsync(string userId, Provider provider, IProviderData providerData)
        {
            var entities = await QueryIntegrationsAsync($"SELECT * FROM i WHERE i.UserId = '{userId}' AND i.Provider = '{provider}'");

            var entity = entities.FirstOrDefault();

            entity.ProviderData = providerData;

            Container container = GetIntegrationsContainer();

            await container.UpsertItemAsync(entity, new PartitionKey(userId));
        }

        private Container GetIntegrationsContainer() => _cosmosClient.GetDatabase("IntegrationsDatabase").GetContainer("IntegrationsContainer");

        private async Task<IEnumerable<IntegrationEntity>> QueryIntegrationsAsync(string query, bool isRetry = false)
        {
            var entities = new List<IntegrationEntity>();

            try
            {
                var container = GetIntegrationsContainer();

                QueryDefinition queryDefinition = new QueryDefinition(query);
                FeedIterator<IntegrationEntity> queryResultSetIterator = container.GetItemQueryIterator<IntegrationEntity>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<IntegrationEntity> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (IntegrationEntity entity in currentResultSet)
                    {
                        entities.Add(entity);
                    }
                }
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                if (!isRetry)
                {
                    var response = await _cosmosClient.CreateDatabaseIfNotExistsAsync("IntegrationsDatabase");
                    await response.Database.CreateContainerIfNotExistsAsync("IntegrationsContainer", "/UserId");
                    return await QueryIntegrationsAsync(query, isRetry: true);
                }
            }

            return entities;
        }
    }
}
