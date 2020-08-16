using System;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.App.Api.Core.Authentication;
using Polly;

namespace MyHealth.App.Api.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiClient<TClient, TImplementation>(this IServiceCollection services, string baseAddress)
            where TClient : class
            where TImplementation : class, TClient
        {
            services
                .AddHttpClient<TClient, TImplementation>(client =>
                {
                    client.BaseAddress = new Uri(baseAddress);
                })
                .AddHttpMessageHandler<DelegationAuthenticationHandler>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1)
                }));

            return services;
        }
    }
}
