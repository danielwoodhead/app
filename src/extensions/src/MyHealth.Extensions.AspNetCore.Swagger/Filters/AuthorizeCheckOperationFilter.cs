using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyHealth.Extensions.AspNetCore.Swagger.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly SwaggerOptions _options;

        public AuthorizeCheckOperationFilter(IOptions<SwaggerOptions> options)
        {
            _options = options.Value;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                }
                            }
                        ] = _options.AuthorizationScopes.Keys.ToArray()
                    }
                };
            }
        }
    }
}
