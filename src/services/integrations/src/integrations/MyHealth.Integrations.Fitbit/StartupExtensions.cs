using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Fitbit.EventHandlers;
using MyHealth.Integrations.Fitbit.Services;
using Polly;

namespace MyHealth.Integrations.Fitbit
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddFitbit(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FitbitSettings>(configuration.GetSection("Fitbit"));
            services.AddTransient<IFitbitService, FitbitService>();
            services.AddTransient<IIntegrationSystemService, FitbitService>();
            services.AddTransient<IFitbitTokenService, FitbitTokenService>();
            services.AddTransient<FitbitBasicAuthenticationHandler>();
            services.AddTransient<FitbitBearerAuthenticationHandler>();
            services.AddTransient<IIntegrationEventHandler, FitbitProviderUpdateEventHandler>();
            services.AddFitbitClient<IFitbitClient, FitbitClient, FitbitBearerAuthenticationHandler>();
            services.AddFitbitClient<IFitbitAuthenticationClient, FitbitAuthenticationClient, FitbitBasicAuthenticationHandler>();

            return services;
        }

        private static IServiceCollection AddFitbitClient<TClient, TImplementation, THandler>(this IServiceCollection services)
            where TClient : class
            where TImplementation : class, TClient
            where THandler : DelegatingHandler
        {
            services
                .AddHttpClient<TClient, TImplementation>((s, client) =>
                {
                    var settings = s.GetService<IOptions<FitbitSettings>>();
                    client.BaseAddress = new Uri(settings.Value.BaseUrl);
                })
                .AddHttpMessageHandler<THandler>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1)
                }));

            return services;
        }
    }
}
