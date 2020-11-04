using Microsoft.AspNetCore.Authorization;

namespace MyHealth.Fhir.Proxy.Authorization
{
    internal class FullAccessRequirement : IAccessRequirement
    {
        public bool IsMet(AuthorizationHandlerContext context) =>
            context.User.HasClaim("scope", "fhir-api");
    }
}
