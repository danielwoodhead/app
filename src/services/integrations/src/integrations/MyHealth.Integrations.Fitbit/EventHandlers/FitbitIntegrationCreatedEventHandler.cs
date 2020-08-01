using System;
using System.Threading.Tasks;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Fitbit.EventHandlers
{
    public class FitbitIntegrationCreatedEventHandler : IIntegrationCreatedEventHandler
    {
        private readonly IFitbitClient _fitbitClient;

        public FitbitIntegrationCreatedEventHandler(IFitbitClient fitbitClient)
        {
            _fitbitClient = fitbitClient ?? throw new ArgumentNullException(nameof(fitbitClient));
        }

        public Provider Provider => Provider.Fitbit;

        public Task RunAsync(IEvent @event)
        {
            // TODO
            // 1. Create FitBit subscription
            // 2. Update integration

            return Task.CompletedTask;
        }
    }
}
