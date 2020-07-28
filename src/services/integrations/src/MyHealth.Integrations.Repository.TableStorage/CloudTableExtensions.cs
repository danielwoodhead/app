using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using MyHealth.Extensions.Azure.Storage.Table;

namespace MyHealth.Integrations.Repository.TableStorage
{
    public static class CloudTableExtensions
    {
        public static async Task<IEnumerable<TEntity>> GetIntegrationsAsync<TEntity>(this CloudTable table, string userId)
        {
            EntityResolver<TableEntity> resolver = (partitionKey, rowKey, timestamp, props, etag) =>
            {
                TableEntity resolvedEntity = null;

                if (rowKey.StartsWith("integrationId_", StringComparison.OrdinalIgnoreCase))
                {
                    resolvedEntity = new IntegrationByIdEntity();
                }
                else if (rowKey.StartsWith("provider_", StringComparison.OrdinalIgnoreCase))
                {
                    resolvedEntity = new IntegrationByProviderEntity();
                }
                else
                {
                    throw new ArgumentException("Unrecognised entity");
                }

                resolvedEntity.PartitionKey = partitionKey;
                resolvedEntity.RowKey = rowKey;
                resolvedEntity.Timestamp = timestamp;
                resolvedEntity.ETag = etag;
                resolvedEntity.ReadEntity(props, null);

                return resolvedEntity;
            };

            var query = new TableQuery<DynamicTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId));

            var entities = await table.QueryAsync(query, resolver);

            return entities.OfType<TEntity>();
        }
    }
}
