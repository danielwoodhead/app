using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MyHealth.Fhir.Proxy.Extensions
{
    internal static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            Claim subClaim = httpContext.User.FindFirst("sub");

            if (subClaim is null)
                return null;

            return subClaim.Value;
        }
    }
}
