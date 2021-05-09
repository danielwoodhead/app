using System.Threading.Tasks;
using AntDesign.Pro.Layout;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Web.App.Areas.DataSharing;
using MyHealth.Web.App.Areas.HealthRecord;
using MyHealth.Web.App.Areas.Integrations;

namespace MyHealth.Web.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddAntDesign();
            builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));

            builder.AddDataSharing();
            builder.AddHealthRecord();
            builder.AddIntegrations();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }
    }
}
