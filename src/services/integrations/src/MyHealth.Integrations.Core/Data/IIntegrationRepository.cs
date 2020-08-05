using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Core.Data
{
    public interface IIntegrationRepository
    {
        Task<Integration> CreateIntegrationAsync(string userId, Provider provider, object data);
        Task DeleteIntegrationAsync(string id, string userId);
        Task<Integration> GetIntegrationAsync(string id, string userId);
        Task<Integration> GetIntegrationAsync(string userId, Provider provider);
        Task<IEnumerable<Integration>> GetIntegrationsAsync(string userId);
        Task UpdateIntegrationAsync(string userId, Provider provider, object integrationData);
    }
}
