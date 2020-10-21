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

    public class ProviderRequest
    {
        public string UserId { get; set; }
        public Provider Provider { get; set; }
        public object Data { get; set; }
    }

    public class ProviderResult
    {
        public Provider Provider { get; set; }
        public string ProviderUserId { get; set; }
        public IProviderData Data { get; set; }
    }
}
