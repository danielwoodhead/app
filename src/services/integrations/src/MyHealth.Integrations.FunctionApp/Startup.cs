using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Integrations.Core.Events;
using MyHealth.Integrations.Fitbit;
using EventHandler = MyHealth.Integrations.Core.Events.EventHandler;

[assembly: FunctionsStartup(typeof(MyHealth.Integrations.FunctionApp.Startup))]

namespace MyHealth.Integrations.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddTransient<IEventHandler, EventHandler>();
            builder.Services.AddFitBit();
        }
    }
}
