using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Fhir;
using MyHealth.Extensions.Logging;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Core.IoMT;
using MyHealth.Integrations.Core.IoMT.Models;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.Clients.Models;
using MyHealth.Integrations.Strava.Models;
using MyHealth.Integrations.Strava.Services;
using Newtonsoft.Json.Linq;

namespace MyHealth.Integrations.Strava.EventHandlers
{
    public class StravaProviderUpdateEventHandler : IIntegrationEventHandler
    {
        private readonly IStravaClient _stravaClient;
        private readonly IStravaAuthenticationService _stravaAuthenticationService;
        private readonly ILogger<StravaProviderUpdateEventHandler> _logger;
        private readonly IIntegrationRepository _integrationRepository;
        private readonly IIoMTDataPublisher _iomtDataPublisher;
        private readonly IFhirClient _fhirClient;

        public StravaProviderUpdateEventHandler(
            IStravaClient stravaClient,
            IStravaAuthenticationService stravaAuthenticationService,
            ILogger<StravaProviderUpdateEventHandler> logger,
            IIntegrationRepository integrationRepository,
            IIoMTDataPublisher iomtDataPublisher,
            IFhirClient fhirClient)
        {
            _stravaClient = stravaClient;
            _stravaAuthenticationService = stravaAuthenticationService;
            _logger = logger;
            _integrationRepository = integrationRepository;
            _iomtDataPublisher = iomtDataPublisher;
            _fhirClient = fhirClient;
        }

        public string EventType => EventTypes.IntegrationProviderUpdate;
        public Provider Provider => Provider.Strava;

        public async Task ProcessAsync(IEvent @event)
        {
            var providerUpdateEvent = (IntegrationProviderUpdateEvent)@event;

            _logger.Information(nameof(StravaProviderUpdateEventHandler), providerUpdateEvent.Properties);

            StravaUpdateNotification stravaUpdate =
                ((JObject)providerUpdateEvent.Data.ProviderData)
                    .ToObject<StravaUpdateNotification>();

            if (stravaUpdate.ObjectType != "activity")
            {
                _logger.LogWarning($"Unsupported Strava object type '{stravaUpdate.ObjectType}'.");
                return;
            }

            string accessToken = await _stravaAuthenticationService.GetAccessTokenAsync(stravaUpdate.OwnerId);

            if (accessToken is null)
            {
                _logger.LogWarning($"Failed to retrieve Strava access token for owner ID '{stravaUpdate.OwnerId}'.");
                return;
            }

            DetailedActivity activity = await _stravaClient.GetActivityAsync(stravaUpdate.ObjectId, accessToken);

            if (activity.Type != ActivityType.Ride)
            {
                _logger.LogWarning($"Unsupported Strava activity type '{activity.Type}'.");
                return;
            }

            await _fhirClient.EnsurePatientDeviceAsync(providerUpdateEvent.Data.UserId);

            IoMTModel ioMTModel = new BikeRide
            {
                Distance = activity.Distance.Value,
                Duration = activity.ElapsedTime.Value,
                DeviceId = providerUpdateEvent.Data.UserId,
                PatientId = providerUpdateEvent.Data.UserId,
                MeasurementDateTime = activity.StartDate.Value
            };

            await _iomtDataPublisher.PublishAsync(ioMTModel);
        }
    }
}
