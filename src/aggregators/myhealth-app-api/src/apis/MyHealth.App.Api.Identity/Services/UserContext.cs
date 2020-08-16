using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using MyHealth.App.Api.Core.Authentication;

namespace MyHealth.App.Api.Identity.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;

        public async Task<string> GetAccessTokenAsync() => await _httpContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token");
    }
}
