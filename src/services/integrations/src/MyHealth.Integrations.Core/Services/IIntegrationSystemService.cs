using System.Threading.Tasks;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Core.Services
{
    public interface IIntegrationSystemService
    {
        Provider Provider { get; }
        Task DeleteIntegrationAsync(string userId);
        Task<ProviderResult> CreateIntegrationAsync(ProviderRequest request);
    }

    public class ProviderRequest : ProviderData
    {
        public string UserId { get; set; }
    }

    public class ProviderResult : ProviderData { }

    public class ProviderData
    {
        public Provider Provider { get; set; }
        public object Data { get; set; }
    }
}
