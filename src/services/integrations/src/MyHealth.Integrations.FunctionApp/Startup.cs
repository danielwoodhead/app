using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using MyHealth.Integrations.Fitbit;

[assembly: FunctionsStartup(typeof(MyHealth.Integrations.FunctionApp.Startup))]

namespace MyHealth.Integrations.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddFitBit();
        }
    }
}
