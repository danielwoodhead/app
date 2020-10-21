using System.Threading.Tasks;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public interface IStravaService
    {
        string GetAuthenticationUri(string redirectUri);
        Task<StravaSubscription> CreateSubscriptionAsync(string callbackUrl);
        Task<StravaSubscription> GetSubscriptionAsync();
        bool ValidateSubscription(string verifyToken);
        Task DeleteSubscriptionAsync();
        Task ProcessUpdateNotification(StravaUpdateNotification stravaUpdateNotification);
    }
}
