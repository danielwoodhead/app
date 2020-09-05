using System.Threading.Tasks;
using MyHealth.Web.Core.Models;

namespace MyHealth.Web.Core.AppApi
{
    public interface IAppApiClient
    {
        Task CreateFitbitIntegrationAsync(string code, string redirectUri);
        Task DeleteIntegrationAsync(string id);
        Task<SearchIntegrationsResponse> GetAllIntegrationsAsync();
        Task<string> GetFitbitAuthenticationUriAsync(string redirectUri);
    }
}
