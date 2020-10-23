using System.Threading.Tasks;
using MyHealth.Integrations.Strava.Clients.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public interface IStravaSubscriptionService
    {
        Task<StravaSubscription> CreateSubscriptionAsync(string callbackUrl);
        Task DeleteSubscriptionAsync();
        Task<StravaSubscription> GetSubscriptionAsync();
        bool ValidateSubscription(string verifyToken);
    }
}
