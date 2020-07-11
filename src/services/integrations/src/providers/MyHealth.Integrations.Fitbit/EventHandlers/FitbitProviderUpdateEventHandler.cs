using System;
using System.Threading.Tasks;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Fitbit.Clients;

namespace MyHealth.Integrations.Fitbit.EventHandlers
{
    public class FitbitProviderUpdateEventHandler : IIntegrationProviderUpdateEventHandler
    {
        private readonly IFitbitClient _fitbitClient;

        public FitbitProviderUpdateEventHandler(IFitbitClient fitbitClient)
        {
            _fitbitClient = fitbitClient ?? throw new ArgumentNullException(nameof(fitbitClient));
        }

        public string Provider => FitbitConstants.Provider;

        public Task RunAsync(IEvent @event)
        {
            // TODO
            // 1. Retrieve updated data from FitBit
            // 2. Send data to IoMT Event Hub

            return Task.CompletedTask;
        }
    }
}
