using System.Threading.Tasks;
using IdentityModel.Client;
using MyHealth.Integrations.Fitbit.Models;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public interface IFitbitClient
    {
        Task<AddFitbitSubscriptionResponse> AddSubscriptionAsync(string subscriptionId, string collectionPath = null, string subscriberId = null);
        Task<TokenResponse> AuthenticateAsync(string code);
    }
}
