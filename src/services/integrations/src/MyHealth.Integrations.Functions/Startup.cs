using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using MyHealth.Integrations.Fitbit;

[assembly: FunctionsStartup(typeof(MyHealth.Integrations.Functions.Startup))]

namespace MyHealth.Integrations.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddFitBit();
        }
    }
}
