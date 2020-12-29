using System.Threading.Tasks;
using MyHealth.Web.App.Areas.Integrations.Models;

namespace MyHealth.Web.App.Areas.Integrations.Client
{
    public interface IIntegrationsClient
    {
        Task DeleteIntegrationAsync(string id);
        Task<GetIntegrationsResponse> GetAllIntegrationsAsync();
        Task CreateFitbitIntegrationAsync(string code, string redirectUri);
        Task<string> GetFitbitAuthenticationUriAsync(string redirectUri);
        Task CreateStravaIntegrationAsync(string code);
        Task<string> GetStravaAuthenticationUriAsync(string redirectUri);
    }
}
