using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.Fhir;
using MyHealth.HealthRecord.Core.Data;

namespace MyHealth.HealthRecord.Data.Fhir
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddFhir(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFhirClient(settings =>
            {
                settings.BaseUrl = configuration.GetSection("FhirServer").GetValue<string>("BaseUrl");
                settings.Timeout = configuration.GetSection("FhirServer").GetValue<TimeSpan>("Timeout");
                settings.AuthenticationMode = AuthenticationMode.AuthenticatedUser;
            });

            services.AddTransient<IObservationsRepository, FhirObservationsRepository>();

            return services;
        }
    }
}
