using System.Threading.Tasks;

namespace MyHealth.Integrations.Fitbit.Services
{
    public interface IFitbitTokenService
    {
        Task<string> GetAccessTokenAsync(string userId);
    }
}
