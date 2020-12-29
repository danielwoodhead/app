using System;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Web.App.Areas.Integrations.Client;

namespace MyHealth.Web.App.Areas.Integrations
{
    internal static class StartupExtensions
    {
        public static WebAssemblyHostBuilder AddIntegrations(this WebAssemblyHostBuilder builder)
        {
            builder.Services
                .AddHttpClient<IIntegrationsClient, IntegrationsClient>(client =>
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
