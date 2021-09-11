using System.Threading.Tasks;

namespace MyHealth.App.Api.Identity.Clients
{
    public interface ITokenClient
    {
        Task<string> GetDelegationTokenAsync(string userId, string userToken);
    }
}
