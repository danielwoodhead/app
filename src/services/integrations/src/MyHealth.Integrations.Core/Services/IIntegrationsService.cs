using System.Threading.Tasks;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Requests;
using MyHealth.Integrations.Models.Response;

namespace MyHealth.Integrations.Core.Services
{
    public interface IIntegrationsService
    {
        Task<SearchIntegrationsResponse> SearchIntegrationsAsync();
        Task<Integration> GetIntegrationAsync(string id);
        Task<Integration> CreateIntegrationAsync(CreateIntegrationRequest request);
        Task DeleteIntegrationAsync(string id);
    }
}
