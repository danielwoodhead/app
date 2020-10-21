using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Core.Data
{
    public interface IIntegrationRepository
    {
        Task<Integration> CreateIntegrationAsync(string userId, Provider provider, string providerUserId, IProviderData data);
        Task DeleteIntegrationAsync(string id, string userId);
        Task<Integration> GetIntegrationAsync(string id, string userId);
        Task<Integration> GetIntegrationAsync(string userId, Provider provider);
        Task<Integration> GetIntegrationAsync(Provider provider, string providerUserId);
        Task<IEnumerable<Integration>> GetIntegrationsAsync(string userId);
        Task UpdateIntegrationAsync(string userId, Provider provider, IProviderData integrationData);
    }
}
