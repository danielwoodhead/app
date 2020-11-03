using System.IdentityModel.Tokens.Jwt;
using MyHealth.Fhir.Proxy.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyHealth.Fhir.Proxy.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // prevent mapping of 'sub' claim
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration.GetSection("Authentication").GetValue<string>("Authority");
                    options.Audience = configuration.GetSection("Authentication").GetValue<string>("Audience");
                });

            return services;
        }

        public static IServiceCollection AddFhirProxy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"));

            services.AddHttpContextAccessor();
            services.AddScoped<IAuthorizationHandler, FhirAuthorizationHandler>();
            services.AddScoped<IAccessRequirement, FullAccessRequirement>();
            services.AddScoped<IAccessRequirement, PatientReadAccessRequirement>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("FhirPolicy", policy =>
                    policy.Requirements.Add(new FhirAuthorizationRequirement()));
            });

            return services;
        }
    }
}
