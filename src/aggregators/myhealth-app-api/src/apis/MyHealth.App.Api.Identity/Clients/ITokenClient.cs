using System.Threading.Tasks;
using IdentityModel.Client;

namespace MyHealth.App.Api.Identity.Clients
{
    public interface ITokenClient
    {
        Task<TokenResponse> GetDelegationTokenAsync(string userToken);
    }
}
