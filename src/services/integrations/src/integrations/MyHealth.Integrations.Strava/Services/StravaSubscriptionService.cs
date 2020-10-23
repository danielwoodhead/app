using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MyHealth.Extensions.Caching.Distributed;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.Clients.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public class StravaSubscriptionService : IStravaSubscriptionService
    {
        private readonly IStravaClient _stravaClient;
        private readonly StravaSettings _stravaSettings;
        private readonly IDistributedCache _cache;

        public StravaSubscriptionService(
            IStravaClient stravaClient,
            IOptions<StravaSettings> settings,
            IDistributedCache cache)
        {
            _stravaClient = stravaClient;
            _stravaSettings = settings.Value;
            _cache = cache;
        }

        public async Task<StravaSubscription> CreateSubscriptionAsync(string callbackUrl)
        {
            StravaSubscription subscription = await _stravaClient.CreateSubscriptionAsync(callbackUrl);
            await CacheSubscriptionAsync(subscription);

            return subscription;
        }

        public async Task DeleteSubscriptionAsync()
        {
            StravaSubscription subscription = await GetSubscriptionAsync();

            if (subscription != null)
            {
                await _stravaClient.DeleteSubscriptionAsync(subscription.Id);
                await _cache.RemoveAsync(Cache.StravaSubscription);
            }
        }

        public async Task<StravaSubscription> GetSubscriptionAsync()
        {
            var cached = await _cache.GetAsync<StravaSubscription>(Cache.StravaSubscription);

            if (cached != null)
            {
                return cached;
            }

            IEnumerable<StravaSubscription> subscriptions = await _stravaClient.GetSubscriptionsAsync();
            StravaSubscription subscription = subscriptions.FirstOrDefault();

            await CacheSubscriptionAsync(subscription);

            return subscription;
        }

        public bool ValidateSubscription(string verifyToken)
        {
            return verifyToken == _stravaSettings.SubscriptionVerifyToken;
        }

        private async Task CacheSubscriptionAsync(StravaSubscription subscription) =>
            await _cache.SetAsync(
                Cache.StravaSubscription,
                subscription,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
    }
}
