using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Requests;

namespace MyHealth.Integrations.Core.Data
{
    public interface IIntegrationsRepository
    {
        Task<Integration> CreateIntegrationAsync(CreateIntegrationRequest request, string userId);
        Task DeleteIntegrationAsync(string id, string userId);
        Task<Integration> GetIntegrationAsync(string id, string userId);
        Task<IEnumerable<Integration>> GetIntegrationsAsync(string userId);
    }
}
