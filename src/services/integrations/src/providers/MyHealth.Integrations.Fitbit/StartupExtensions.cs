using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Fitbit.EventHandlers;
using Polly;

namespace MyHealth.Integrations.Fitbit
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddFitBit(this IServiceCollection services)
        {
            services
                .AddConfiguration()
                .AddEventHandlers()
                .AddClients();

            return services;
        }

        private static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            services.AddOptions<FitbitSettings>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    options.BaseUrl = configuration["FitbitBaseUrl"];
                    options.VerificationCode = configuration["FitbitVerificationCode"];
                });

            return services;
        }

        private static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.AddTransient<IIntegrationCreatedEventHandler, FitbitIntegrationCreatedEventHandler>();
            services.AddTransient<IIntegrationProviderUpdateEventHandler, FitbitProviderUpdateEventHandler>();

            return services;
        }

        private static IServiceCollection AddClients(this IServiceCollection services)
        {
            services
                .AddHttpClient<IFitbitClient, FitbitClient>((s, client) =>
                {
                    var settings = s.GetService<IOptions<FitbitSettings>>();

                    client.BaseAddress = new Uri(settings.Value.BaseUrl);
                })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }));

            return services;
        }
    }
}
