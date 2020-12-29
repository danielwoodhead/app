using System;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Web.App.Areas.DataSharing.Client;

namespace MyHealth.Web.App.Areas.DataSharing
{
    internal static class StartupExtensions
    {
        public static WebAssemblyHostBuilder AddDataSharing(this WebAssemblyHostBuilder builder)
        {
            builder.Services
                .AddHttpClient<IDataSharingClient, DataSharingClient>(client =>
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
