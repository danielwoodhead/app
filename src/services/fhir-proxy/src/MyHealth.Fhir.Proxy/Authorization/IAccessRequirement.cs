using Microsoft.AspNetCore.Authorization;

namespace MyHealth.Fhir.Proxy.Authorization
{
    internal interface IAccessRequirement
    {
        bool IsMet(AuthorizationHandlerContext context);
    }
}
