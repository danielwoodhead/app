using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Integrations.Strava.Clients.Models;

namespace MyHealth.Integrations.Strava.Clients
{
    public interface IStravaClient
    {
        Task<TokenResponse> AuthenticateAsync(string code);
        Task<TokenResponse> RefreshTokenAsync(string refreshToken);
        Task<StravaSubscription> CreateSubscriptionAsync(string callbackUrl);
        Task<DetailedActivity> GetActivityAsync(long activityId, string accessToken);
        Task<IEnumerable<StravaSubscription>> GetSubscriptionsAsync();
        Task DeleteSubscriptionAsync(int subscriptionId);
    }
}
