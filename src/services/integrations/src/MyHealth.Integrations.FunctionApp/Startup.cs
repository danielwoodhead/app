using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Integrations.Core.Events;
using MyHealth.Integrations.Data.Cosmos;
using MyHealth.Integrations.Fitbit;
using MyHealth.Integrations.IoMT.EventHub;
using MyHealth.Integrations.Strava;
using MyHealth.Integrations.Utility;
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

            ServiceProvider services = builder.Services.BuildServiceProvider();
            var configuration = services.GetRequiredService<IConfiguration>();

            builder.Services.AddTransient<IEventHandler, EventHandler>();
            builder.Services.AddIoMTEventHub(configuration);
            builder.Services.AddCosmos(configuration);
            builder.Services.AddUtility();

            // integrations
            builder.Services.AddFitbit(configuration);
            builder.Services.AddFitbitEventHandlers();
            builder.Services.AddStrava(configuration);
            builder.Services.AddStravaEventHandlers();
        }
    }
}
