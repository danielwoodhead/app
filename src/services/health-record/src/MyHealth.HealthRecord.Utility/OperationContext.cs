using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace MyHealth.HealthRecord.Utility
{
    public class OperationContext : IOperationContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OperationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string OperationId => Activity.Current?.RootId;

        public string UserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;

        public async Task<string> GetAccessTokenAsync() => await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
    }
}
