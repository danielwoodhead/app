using MyHealth.Fhir.Proxy.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace MyHealth.Fhir.Proxy.Authorization
{
    internal class PatientReadAccessRequirement : IAccessRequirement
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientReadAccessRequirement(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsMet(AuthorizationHandlerContext context)
        {
            if (!context.User.HasClaim("scope", "fhir-api:patient:read"))
                return false;

            HttpContext httpContext = _httpContextAccessor.HttpContext;
            string userId = httpContext.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            string requestPath = httpContext.Request.Path.Value;

            if (requestPath.StartsWith($"/Patient/{userId}"))
            {
                return true;
            }

            return false;
        }
    }
}
