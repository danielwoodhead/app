using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.EventHandlers;
using MyHealth.Integrations.Strava.Services;

namespace MyHealth.Integrations.Strava
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddStrava(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StravaSettings>(configuration.GetSection("Strava"));
            services.AddTransient<IIntegrationSystemService, StravaIntegrationService>();
            services.AddTransient<IStravaAuthenticationService, StravaAuthenticationService>();
            services.AddTransient<IStravaSubscriptionService, StravaSubscriptionService>();
            services.AddTransient<IStravaUpdateService, StravaUpdateService>();
            services.AddTransient<IIntegrationProviderUpdateEventHandler, StravaProviderUpdateEventHandler>();

            services.AddHttpClient<IStravaClient, StravaClient>((s, client) =>
            {
                var settings = s.GetService<IOptions<StravaSettings>>();
                client.BaseAddress = new Uri(settings.Value.ApiUrl);
            });

            return services;
        }
    }
}
