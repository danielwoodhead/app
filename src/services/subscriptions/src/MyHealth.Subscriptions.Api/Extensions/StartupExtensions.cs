using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.AspNetCore.Swagger;

namespace MyHealth.Subscriptions.Api.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // prevent mapping of 'sub' claim
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration["Authentication:Authority"];
                    options.Audience = configuration["Authentication:Audience"];
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Subscriptions", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "myhealth-subscriptions-api");
                });
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMyHealthSwagger(options =>
            {
                options.ApiName = "MyHealth Subscriptions API";
                options.AuthorizationScopes = new Dictionary<string, string>
                {
                    { "myhealth-subscriptions-api", "MyHealth Subscriptions API" }
                };
                options.AuthorizationUrl = configuration["Swagger:AuthorizationUrl"];
                options.TokenUrl = configuration["Swagger:TokenUrl"];
            });

            return services;
        }
    }
}
