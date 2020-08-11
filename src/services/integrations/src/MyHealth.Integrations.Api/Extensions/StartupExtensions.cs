using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.DependencyInjection;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Events.ApplicationInsights;
using MyHealth.Extensions.Events.Azure.EventGrid;
using MyHealth.Integrations.Core.Services;

namespace MyHealth.Integrations.Api.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddVersionAwareSwagger(options =>
            {
                options.ApiName = "MyHealth Integrations API";
            });

            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
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

        public static IServiceCollection AddIntegrationsCore(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.Configure<EventGridSettings>(configuration.GetSection("EventGrid"));

            services.AddTransient<IIntegrationService, IntegrationService>();
            services.AddTransient<IEventPublisher, ApplicationInsightsEventPublisher>();
            services.AddSingleton<IEventPublisher, EventGridEventPublisher>();
            services.AddComposite<IEventPublisher, CompositeEventPublisher>();

            return services;
        }
    }
}
