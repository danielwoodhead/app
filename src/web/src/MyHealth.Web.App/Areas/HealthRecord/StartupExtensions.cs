using System;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Web.App.Areas.HealthRecord.Client;

namespace MyHealth.Web.App.Areas.HealthRecord
{
    internal static class StartupExtensions
    {
        public static WebAssemblyHostBuilder AddHealthRecord(this WebAssemblyHostBuilder builder)
        {
            builder.Services
                .AddHttpClient<IHealthRecordClient, HealthRecordClient>(client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration["AppApi:BaseUrl"]);
                })
                .AddHttpMessageHandler(sp =>
                {
                    return sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { builder.Configuration["AppApi:BaseUrl"] },
                            scopes: new[] { "myhealth-app-api" });
                });

            return builder;
        }
    }
}
