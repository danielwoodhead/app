using System.Threading.Tasks;
using MyHealth.Integrations.Fitbit.Models;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public interface IFitbitClient
    {
        Task<AddFitbitSubscriptionResponse> AddSubscriptionAsync(string subscriptionId, string collectionPath = null, string subscriberId = null);
    }
}
