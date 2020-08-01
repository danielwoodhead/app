using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using MyHealth.Extensions.Azure.Storage.Table;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Requests;

namespace MyHealth.Integrations.Data.TableStorage
{
    public class TableStorageIntegrationsRepository : IIntegrationsRepository
    {
        private readonly CloudTable _table;
        private readonly TableStorageSettings _settings;

        public TableStorageIntegrationsRepository(IOptions<TableStorageSettings> settings)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

            _table = CloudStorageAccount.Parse(_settings.ConnectionString)
                .CreateCloudTableClient()
                .GetTableReference(_settings.IntegrationsTableName);
        }

        public async Task<Integration> CreateIntegrationAsync(CreateIntegrationRequest request, string userId)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var e1 = new IntegrationByIdEntity(userId, Guid.NewGuid().ToString(), request.Provider);
            var e2 = new IntegrationByProviderEntity(userId, request.Provider, request.Data);

            await _table.BatchInsertAsync(e1, e2);

            return e1.Map();
        }

        public async Task DeleteIntegrationAsync(string id, string userId)
        {
            var e1 = await _table.RetrieveAsync<IntegrationByIdEntity>(userId, IntegrationByIdEntity.ToRowKey(id));
            var e2 = await _table.RetrieveAsync<IntegrationByProviderEntity>(userId, IntegrationByProviderEntity.ToRowKey(e1.Provider));

            await _table.BatchDeleteAsync(e1, e2);
        }

        public async Task<Integration> GetIntegrationAsync(string id, string userId)
        {
            var entity = await _table.RetrieveAsync<IntegrationByIdEntity>(userId, IntegrationByIdEntity.ToRowKey(id));

            if (entity == null)
                return null;

            return entity.Map();
        }

        public async Task<Integration> GetIntegrationAsync(Provider provider, string userId)
        {
            var entity = await _table.RetrieveAsync<IntegrationByProviderEntity>(userId, IntegrationByProviderEntity.ToRowKey(provider.ToString()));

            if (entity == null)
                return null;

            return entity.Map();
        }

        public async Task<IEnumerable<Integration>> GetIntegrationsAsync(string userId)
        {
            var entities = await _table.GetIntegrationsAsync<IntegrationByIdEntity>(userId);

            return entities.Select(e => e.Map());
        }
    }
}
