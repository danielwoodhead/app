using System.Threading.Tasks;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Fitbit.Models;

namespace MyHealth.Integrations.Tests.Mocks
{
    public class MockFitbitClient : IFitbitClient
    {
        public ResourceContainer ResourceToReturn { get; set; }

        public Task<AddFitbitSubscriptionResponse> AddSubscriptionAsync(string subscriptionId, string accessToken, string collectionPath = null, string subscriberId = null) => throw new System.NotImplementedException();
        public Task DeleteSubscriptionAsync(string subscriptionId, string collectionPath = null, string subscriberId = null) => throw new System.NotImplementedException();
        public Task<ResourceContainer> GetResourceAsync(string ownerType, string ownerId, string collectionType, System.DateTime date, string accessToken)
        {
            return Task.FromResult(ResourceToReturn);
        }
    }
}
