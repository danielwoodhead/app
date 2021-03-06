﻿using System;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Fhir;
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
    public class FitbitProviderUpdateEventHandler : IIntegrationEventHandler
    {
        private readonly IFitbitClient _fitbitClient;
        private readonly IFitbitTokenService _fitbitTokenService;
        private readonly IIoMTDataPublisher _iomtDataPublisher;
        private readonly IFhirClient _fhirClient;
        private readonly ILogger<FitbitProviderUpdateEventHandler> _logger;

        public FitbitProviderUpdateEventHandler(
            IFitbitClient fitbitClient,
            IFitbitTokenService fitbitTokenService,
            IIoMTDataPublisher iomtDataPublisher,
            IFhirClient fhirClient,
            ILogger<FitbitProviderUpdateEventHandler> logger)
        {
            _fitbitClient = fitbitClient ?? throw new ArgumentNullException(nameof(fitbitClient));
            _fitbitTokenService = fitbitTokenService ?? throw new ArgumentNullException(nameof(fitbitTokenService));
            _iomtDataPublisher = iomtDataPublisher ?? throw new ArgumentNullException(nameof(iomtDataPublisher));
            _fhirClient = fhirClient ?? throw new ArgumentNullException(nameof(fhirClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string EventType => EventTypes.IntegrationProviderUpdate;
        public Provider Provider => Provider.Fitbit;

        public async Task ProcessAsync(IEvent @event)
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

            if (resource.Body is null)
            {
                _logger.LogWarning($"Data mapping not supported: {JsonConvert.SerializeObject(resource)}");
                return;
            }

            await _fhirClient.EnsurePatientDeviceAsync(providerUpdateEvent.Data.UserId);

            IoMTModel iomtModel = new BodyWeight
            {
                Weight = resource.Body.Weight,
                MeasurementDateTime = fitbitUpdate.Date,
                DeviceId = providerUpdateEvent.Data.UserId,
                PatientId = providerUpdateEvent.Data.UserId
            };

            await _iomtDataPublisher.PublishAsync(iomtModel);
        }
    }
}
