using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Fitbit.EventHandlers;
using MyHealth.Integrations.Fitbit.Services;
using Polly;

namespace MyHealth.Integrations.Fitbit
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddFitBit(this IServiceCollection services)
        {
            services.AddOptions<FitbitSettings>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    options.BaseUrl = configuration["Fitbit:BaseUrl"];
                    options.VerificationCode = configuration["Fitbit:VerificationCode"];
                    options.ClientId = configuration["Fitbit:ClientId"];
                    options.ClientSecret = configuration["Fitbit:ClientSecret"];
                });

            services.AddTransient<IFitbitService, FitbitService>();
            services.AddTransient<IIntegrationCreatedEventHandler, FitbitIntegrationCreatedEventHandler>();
            services.AddTransient<IIntegrationProviderUpdateEventHandler, FitbitProviderUpdateEventHandler>();

            services.AddTransient<FitbitAuthenticationHandler>();
            services
                .AddHttpClient<IFitbitClient, FitbitClient>((s, client) =>
                {
                    var settings = s.GetService<IOptions<FitbitSettings>>();
                    client.BaseAddress = new Uri(settings.Value.BaseUrl);
                })
                .AddHttpMessageHandler<FitbitAuthenticationHandler>()
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
