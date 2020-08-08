﻿using System;
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
            services.AddTransient<IIntegrationSystemService, FitbitService>();
            services.AddTransient<IFitbitTokenService, FitbitTokenService>();
            services.AddTransient<IIntegrationProviderUpdateEventHandler, FitbitProviderUpdateEventHandler>();
            services.AddTransient<FitbitBasicAuthenticationHandler>();
            services.AddTransient<FitbitBearerAuthenticationHandler>();

            services
                .AddHttpClient<IFitbitClient, FitbitClient>((s, client) =>
                {
                    var settings = s.GetService<IOptions<FitbitSettings>>();
                    client.BaseAddress = new Uri(settings.Value.BaseUrl);
                })
                .AddHttpMessageHandler<FitbitBearerAuthenticationHandler>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1)
                }));
            services
                .AddHttpClient<IFitbitAuthenticationClient, FitbitAuthenticationClient>((s, client) =>
                {
                    var settings = s.GetService<IOptions<FitbitSettings>>();
                    client.BaseAddress = new Uri(settings.Value.BaseUrl);
                })
                .AddHttpMessageHandler<FitbitBasicAuthenticationHandler>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1)
                }));

            return services;
        }
    }
}
