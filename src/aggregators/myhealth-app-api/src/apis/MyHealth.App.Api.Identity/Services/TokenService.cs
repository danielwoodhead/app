using System.Threading.Tasks;
using MyHealth.App.Api.Core.Authentication;
using MyHealth.App.Api.Identity.Clients;

namespace MyHealth.App.Api.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenClient _identityClient;
        private readonly IUserContext _userContext;

        public TokenService(ITokenClient identityClient, IUserContext userContext)
        {
            _identityClient = identityClient;
            _userContext = userContext;
        }

        public async Task<string> GetDelegationTokenAsync()
        {
            string userId = _userContext.UserId;
            string userToken = await _userContext.GetAccessTokenAsync();

            return await _identityClient.GetDelegationTokenAsync(userId, userToken);
        }
    }
}
