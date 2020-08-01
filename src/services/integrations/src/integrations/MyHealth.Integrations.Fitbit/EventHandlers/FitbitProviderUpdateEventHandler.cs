using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;

namespace MyHealth.Integrations.Fitbit.EventHandlers
{
    public class FitbitProviderUpdateEventHandler : IIntegrationProviderUpdateEventHandler
    {
        private readonly IFitbitClient _fitbitClient;
        private readonly ILogger<FitbitProviderUpdateEventHandler> _logger;

        public FitbitProviderUpdateEventHandler(IFitbitClient fitbitClient, ILogger<FitbitProviderUpdateEventHandler> logger)
        {
            _fitbitClient = fitbitClient ?? throw new ArgumentNullException(nameof(fitbitClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Provider Provider => Provider.Fitbit;

        public Task RunAsync(IEvent @event)
        {
            var providerUpdateEvent = (IntegrationProviderUpdateEvent)@event;

            // TODO
            // 1. Retrieve updated data from FitBit
            // 2. Send data to IoMT Event Hub

            _logger.LogInformation($"FitbitProviderUpdateEventHandler: Provider={providerUpdateEvent.Data.Provider}, UserId={providerUpdateEvent.Data.UserId}.");

            return Task.CompletedTask;
        }
    }
}
