using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Logging;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.Models;
using Newtonsoft.Json.Linq;

namespace MyHealth.Integrations.Strava.EventHandlers
{
    public class StravaProviderUpdateEventHandler : IIntegrationProviderUpdateEventHandler
    {
        private readonly IStravaClient _stravaClient;
        private readonly ILogger<StravaProviderUpdateEventHandler> _logger;
        private readonly IIntegrationRepository _integrationRepository;

        public StravaProviderUpdateEventHandler(
            IStravaClient stravaClient,
            ILogger<StravaProviderUpdateEventHandler> logger,
            IIntegrationRepository integrationRepository)
        {
            _stravaClient = stravaClient;
            _logger = logger;
            _integrationRepository = integrationRepository;
        }

        public Provider Provider => Provider.Strava;

        public async Task RunAsync(IEvent @event)
        {
            var providerUpdateEvent = (IntegrationProviderUpdateEvent)@event;

            _logger.Information(nameof(StravaProviderUpdateEventHandler), providerUpdateEvent.Properties);

            StravaUpdateNotification stravaUpdate =
                ((JObject)providerUpdateEvent.Data.ProviderData)
                    .ToObject<StravaUpdateNotification>();

            var integration = await _integrationRepository.GetIntegrationAsync(Provider, stravaUpdate.OwnerId.ToString());

            if (integration is null)
            {
                _logger.LogError($"Failed to process Strava update event. Integration not found for OwnerId '{stravaUpdate.OwnerId}'.");
                return;
            }

            _logger.LogInformation($"Integration found!"); // TEMP

            // TODO: get new data from Strava
            // TODO: convert data to IoMT model
            // TODO: publish data to IoMT
        }
    }
}
