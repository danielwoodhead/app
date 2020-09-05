using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Web.Core.AppApi;

namespace MyHealth.Web.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services
                .AddHttpClient<IAppApiClient, AppApiClient>(client =>
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

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }
    }
}
