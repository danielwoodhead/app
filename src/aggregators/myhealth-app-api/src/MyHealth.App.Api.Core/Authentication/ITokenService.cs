using System.Threading.Tasks;

namespace MyHealth.App.Api.Core.Authentication
{
    public interface ITokenService
    {
        Task<string> GetDelegationTokenAsync();
    }
}
