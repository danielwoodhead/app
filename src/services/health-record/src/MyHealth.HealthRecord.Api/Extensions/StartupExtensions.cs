using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.DependencyInjection;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Events.ApplicationInsights;
using MyHealth.Extensions.Events.Azure.EventGrid;

namespace MyHealth.HealthRecord.Api.Extensions
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

            return services;
        }

        public static IServiceCollection AddEvents(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EventGridSettings>(configuration.GetSection("EventGrid"));
            services.AddSingleton<IEventPublisher, EventGridEventPublisher>();
            services.AddTransient<IEventPublisher, ApplicationInsightsEventPublisher>();
            services.AddComposite<IEventPublisher, CompositeEventPublisher>();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMyHealthSwagger(options =>
            {
                options.ApiName = "MyHealth Health Record API";
                options.AuthorizationScopes = new Dictionary<string, string>
                {
                    { "myhealth-healthrecord-api", "MyHealth Health Record API" },
                    { "fhir-api", "FHIR API" }
                };
                options.AuthorizationUrl = configuration["Swagger:AuthorizationUrl"];
                options.TokenUrl = configuration["Swagger:TokenUrl"];
            });

            return services;
        }
    }
}
