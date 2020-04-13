using System.Threading.Tasks;
using MyHealth.Mobile.Core.Models.Identity;

namespace MyHealth.Mobile.Core.Services.Identity
{
    public interface IIdentityService
    {
        string CreateAuthorizationRequest();
        string CreateLogoutRequest(string token);
        Task<UserToken> GetTokenAsync(string code);
    }
}
