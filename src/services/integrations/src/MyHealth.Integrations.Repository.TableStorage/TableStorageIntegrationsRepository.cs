using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using MyHealth.Extensions.Azure.Storage.Table;
using MyHealth.Integrations.Core.Repository;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Requests;

namespace MyHealth.Integrations.Repository.TableStorage
{
    public class TableStorageIntegrationsRepository : IIntegrationsRepository
    {
        private readonly CloudTableClient _cloudTableClient;
        private readonly TableStorageSettings _settings;

        public TableStorageIntegrationsRepository(IOptions<TableStorageSettings> settings)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

            _cloudTableClient = CloudStorageAccount.Parse(_settings.ConnectionString).CreateCloudTableClient();
        }

        private CloudTable Table => _cloudTableClient.GetTableReference(_settings.IntegrationsTableName);

        public async Task<Integration> CreateIntegrationAsync(CreateIntegrationRequest request, string userId)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var entity = new IntegrationEntity(
                id: Guid.NewGuid().ToString(),
                userId: userId);

            entity.Provider = request.Provider.ToString();

            await Table.InsertAsync(entity);

            return entity.Map();
        }

        public async Task DeleteIntegrationAsync(string id, string userId)
        {
            var entity = await Table.RetrieveAsync<IntegrationEntity>(userId, id);

            await Table.DeleteAsync(entity);
        }

        public async Task<Integration> GetIntegrationAsync(string id, string userId)
        {
            var entity = await Table.RetrieveAsync<IntegrationEntity>(userId, id);

            if (entity == null)
                return null;

            return entity.Map();
        }

        public async Task<IEnumerable<Integration>> GetIntegrationsAsync(string userId)
        {
            var entities = await Table.RetrievePartitionAsync<IntegrationEntity>(userId);

            return entities.Select(e => e.Map());
        }

        public async Task<Integration> UpdateIntegrationAsync(string id, string userId, UpdateIntegrationRequest request)
        {
            var entity = new IntegrationEntity(
                id: id,
                userId: userId);

            await Table.InsertOrMergeAsync(entity);

            return entity.Map();
        }
    }
}
