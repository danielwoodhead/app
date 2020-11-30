using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Tests.Mocks
{
    public class MockIntegrationRepository : IIntegrationRepository
    {
        public Integration Integration { get; set; }

        public Task<Integration> CreateIntegrationAsync(string userId, Provider provider, string providerUserId, IProviderData data) => throw new System.NotImplementedException();
        public Task DeleteIntegrationAsync(string id, string userId) => throw new System.NotImplementedException();
        public Task<Integration> GetIntegrationAsync(string id, string userId) => throw new System.NotImplementedException();
        public Task<Integration> GetIntegrationAsync(string userId, Provider provider) => Task.FromResult(Integration);
        public Task<Integration> GetIntegrationAsync(Provider provider, string providerUserId) => throw new System.NotImplementedException();
        public Task<IEnumerable<Integration>> GetIntegrationsAsync(string userId) => throw new System.NotImplementedException();
        public Task UpdateIntegrationAsync(string userId, Provider provider, IProviderData integrationData) => throw new System.NotImplementedException();
    }
}
