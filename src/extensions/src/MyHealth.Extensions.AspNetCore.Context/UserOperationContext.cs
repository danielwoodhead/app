using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace MyHealth.Extensions.AspNetCore.Context
{
    public class UserOperationContext : OperationContext, IUserOperationContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserOperationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string UserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;

        public async Task<string> GetAccessTokenAsync() => await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
    }
}
