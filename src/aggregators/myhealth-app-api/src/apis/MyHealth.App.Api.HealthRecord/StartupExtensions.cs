using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.App.Api.HealthRecord.Services;
using MyHealth.Extensions.AspNetCore.Context;
using MyHealth.Extensions.Fhir;

namespace MyHealth.App.Api.HealthRecord
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddHealthRecordApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddContext();
            services.AddFhirClient<FhirAuthenticationService>(settings =>
            {
                settings.BaseUrl = configuration.GetSection("FhirApi").GetValue<string>("BaseAddress");
                settings.Timeout = configuration.GetSection("FhirApi").GetValue<TimeSpan>("Timeout");
            });
            services.AddTransient<IHealthRecordService, HealthRecordService>();

            return services;
        }
    }
}
