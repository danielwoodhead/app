using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MyHealth.Fhir.Proxy.Authorization
{
    internal class FhirAuthorizationHandler : AuthorizationHandler<FhirAuthorizationRequirement>
    {
        private readonly IEnumerable<IAccessRequirement> _accessRequirements;

        public FhirAuthorizationHandler(IEnumerable<IAccessRequirement> accessRequirements)
        {
            _accessRequirements = accessRequirements;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            FhirAuthorizationRequirement requirement)
        {
            foreach (IAccessRequirement accessRequirement in _accessRequirements)
            {
                if (accessRequirement.IsMet(context))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            return Task.CompletedTask;
        }
    }

    public class FhirAuthorizationRequirement : IAuthorizationRequirement { }
}
