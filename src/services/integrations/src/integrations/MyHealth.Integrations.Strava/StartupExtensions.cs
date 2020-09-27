using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.Services;

namespace MyHealth.Integrations.Strava
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddStrava(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StravaSettings>(configuration.GetSection("Strava"));
            services.AddTransient<IStravaService, StravaService>();
            services.AddTransient<IIntegrationSystemService, StravaService>();

            services.AddHttpClient<IStravaClient, StravaClient>((s, client) =>
            {
                var settings = s.GetService<IOptions<StravaSettings>>();
                client.BaseAddress = new Uri(settings.Value.ApiUrl);
            });

            return services;
        }
    }
}
