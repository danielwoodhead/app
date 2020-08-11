using System;
using System.Threading.Tasks;
using MyHealth.Integrations.Fitbit.Models;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public interface IFitbitClient
    {
        Task<AddFitbitSubscriptionResponse> AddSubscriptionAsync(string subscriptionId, string accessToken, string collectionPath = null, string subscriberId = null);
        Task DeleteSubscriptionAsync(string subscriptionId, string collectionPath = null, string subscriberId = null);
        Task<ResourceContainer> GetResourceAsync(string ownerType, string ownerId, string collectionType, DateTime date, string accessToken);
    }
}
