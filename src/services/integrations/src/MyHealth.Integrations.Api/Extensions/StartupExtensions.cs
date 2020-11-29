using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.DependencyInjection;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Events.ApplicationInsights;
using MyHealth.Extensions.Events.Azure.EventGrid;
using MyHealth.Extensions.Fhir;

namespace MyHealth.Integrations.Api.Extensions
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

        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IntegrationsAdmin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "myhealth-integrations-api:admin");
                });
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

        public static IServiceCollection AddFhir(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFhirClient(settings =>
            {
                settings.BaseUrl = configuration.GetSection("Fhir").GetValue<string>("BaseUrl");
                settings.Timeout = configuration.GetSection("Fhir").GetValue<TimeSpan>("Timeout");
                settings.AuthenticationMode = AuthenticationMode.ClientCredentials;
                settings.AuthenticationTokenEndpoint = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationTokenEndpoint");
                settings.AuthenticationClientId = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationClientId");
                settings.AuthenticationClientSecret = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationClientSecret");
                settings.AuthenticationScope = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationScope");
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMyHealthSwagger(options =>
            {
                options.ApiName = "MyHealth Integrations API";
                options.AuthorizationScopes = new Dictionary<string, string>
                {
                    { "myhealth-integrations-api", "MyHealth Integrations API" }
                };
                options.AuthorizationUrl = configuration["Swagger:AuthorizationUrl"];
                options.TokenUrl = configuration["Swagger:TokenUrl"];
            });

            return services;
        }
    }
}
