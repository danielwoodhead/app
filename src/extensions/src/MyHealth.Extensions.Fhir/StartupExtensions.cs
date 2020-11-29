using System;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.Fhir.Authentication;
using MyHealth.Extensions.Fhir.Client;

namespace MyHealth.Extensions.Fhir
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddFhirClient(this IServiceCollection services, Action<FhirClientSettings> configure)
        {
            var settings = new FhirClientSettings();
            configure(settings);
            services.Configure(configure);

            services.AddTransient<IFhirClientFactory, FhirClientFactory>();
            services.AddTransient(s =>
            {
                return s.GetRequiredService<IFhirClientFactory>().Create();
            });

            switch (settings.AuthenticationMode)
            {
                case AuthenticationMode.AuthenticatedUser:
                    services.AddScoped<IAuthenticationService, AuthenticatedUserAuthenticationService>();
                    break;
                case AuthenticationMode.ClientCredentials:
                    services.AddMemoryCache();
                    services.AddHttpClient<IAuthenticationService, ClientCredentialsAuthenticationService>();
                    break;
                default:
                    throw new ArgumentException($"Unsupported AuthenticationMode: '{settings.AuthenticationMode}'");
            }

            return services;
        }
    }
}
