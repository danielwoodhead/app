using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Logging;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Core.IoMT;
using MyHealth.Integrations.Core.IoMT.Models;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Fitbit.Services;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyHealth.Integrations.Fitbit.EventHandlers
{
    public class FitbitProviderUpdateEventHandler : IIntegrationProviderUpdateEventHandler
    {
        private readonly IFitbitClient _fitbitClient;
        private readonly IFitbitTokenService _fitbitTokenService;
        private readonly IIoMTDataPublisher _iomtDataPublisher;
        private readonly ILogger<FitbitProviderUpdateEventHandler> _logger;

        public FitbitProviderUpdateEventHandler(
            IFitbitClient fitbitClient,
            IFitbitTokenService fitbitTokenService,
            IIoMTDataPublisher iomtDataPublisher,
            ILogger<FitbitProviderUpdateEventHandler> logger)
        {
            _fitbitClient = fitbitClient ?? throw new ArgumentNullException(nameof(fitbitClient));
            _fitbitTokenService = fitbitTokenService ?? throw new ArgumentNullException(nameof(fitbitTokenService));
            _iomtDataPublisher = iomtDataPublisher ?? throw new ArgumentNullException(nameof(iomtDataPublisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Provider Provider => Provider.Fitbit;

        public async Task RunAsync(IEvent @event)
        {
            var providerUpdateEvent = (IntegrationProviderUpdateEvent)@event;

            _logger.Information(nameof(FitbitProviderUpdateEventHandler), providerUpdateEvent.Properties);

            FitbitUpdateNotification fitbitUpdate =
                ((JObject)providerUpdateEvent.Data.ProviderData)
                    .ToObject<FitbitUpdateNotification>();

            string accessToken = await _fitbitTokenService.GetAccessTokenAsync(providerUpdateEvent.Data.UserId);
            ResourceContainer resource = await _fitbitClient.GetResourceAsync(
                ownerType: fitbitUpdate.OwnerType,
                ownerId: fitbitUpdate.OwnerId,
                collectionType: fitbitUpdate.CollectionType,
                date: fitbitUpdate.Date,
                accessToken: accessToken);

            IoMTModel model = ConvertToIoMTModel(resource, fitbitUpdate);

            if (model == null)
            {
                _logger.LogWarning($"Data mapping not supported: {JsonConvert.SerializeObject(resource)}");
                return;
            }

            await _iomtDataPublisher.PublishAsync(model);
        }

        private static IoMTModel ConvertToIoMTModel(ResourceContainer resource, FitbitUpdateNotification fitbitUpdate)
        {
            if (resource.Body == null)
                return null;

            return new BodyWeight
            {
                Weight = resource.Body.Weight,
                MeasurementDateTime = fitbitUpdate.Date,
                DeviceId = fitbitUpdate.SubscriptionId,
                PatientId = fitbitUpdate.SubscriptionId
            };
        }
    }
}
