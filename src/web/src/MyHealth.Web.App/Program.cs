using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Web.Core.Integrations;

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
                .AddHttpClient<IIntegrationsClient, IntegrationsClient>(client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration["Integrations:BaseUrl"]);
                })
                .AddHttpMessageHandler(sp =>
                {
                    return sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { builder.Configuration["Integrations:BaseUrl"] },
                            scopes: new[] { "integrations-api" });
                });

            builder.Services.AddTransient<IFitbitClient, FitbitClient>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }
    }
}
