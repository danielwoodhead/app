using System.Threading.Tasks;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Response;

namespace MyHealth.Integrations.Core.Services
{
    public interface IIntegrationService
    {
        Task<GetIntegrationsResponse> GetIntegrationsAsync();
        Task<Integration> GetIntegrationAsync(string id);
        Task<Integration> GetIntegrationAsync(string userId, Provider provider);
        Task<Integration> CreateIntegrationAsync(ProviderRequest request);
        Task DeleteIntegrationAsync(string id);
        Task UpdateIntegrationAsync(string userId, Provider fitbit, object integrationData);
    }
}
