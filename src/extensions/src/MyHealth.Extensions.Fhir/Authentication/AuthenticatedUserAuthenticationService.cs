using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace MyHealth.Extensions.Fhir.Authentication
{
    public class AuthenticatedUserAuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedUserAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetAccessTokenAsync() => await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
    }
}
