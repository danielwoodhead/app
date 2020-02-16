﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Options;
using MyHealth.Observations.Core.Events;
using MyHealth.Observations.Integration.Events.Base;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Events;
using MyHealth.Observations.Models.Events.Base;
using MyHealth.Observations.Utility;

namespace MyHealth.Observations.Integration.Events
{
    public class EventGridEventPublisher : IEventPublisher, IDisposable
    {
        private const string EventDataVersion = "1.0";
        private const string EventSourceSystem = "myhealth:observations:api";
        private readonly EventGridSettings _settings;
        private readonly EventGridClient _eventGridClient;
        private readonly IOperationContext _operationContext;

        public EventGridEventPublisher(
            IOptions<EventGridSettings> settings,
            IOperationContext operationContext)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _eventGridClient = new EventGridClient(new TopicCredentials(_settings.TopicKey));
            _operationContext = operationContext;
        }

        public async Task PublishObservationCreatedEvent(Observation observation)
        {
            if (!_settings.Enabled)
                return;

            if (observation == null)
                throw new ArgumentNullException(nameof(observation));

            ObservationCreatedEvent @event = CreateObservationCreatedEvent(observation);

            await PublishEventAsync(@event.ToEventGridEvent());
        }

        private ObservationCreatedEvent CreateObservationCreatedEvent(Observation observation) =>
            new ObservationCreatedEvent(
                id: Guid.NewGuid().ToString(),
                subject: observation.Id,
                eventTime: DateTime.UtcNow,
                dataVersion: EventDataVersion,
                data: new EventData
                {
                    OperationId = _operationContext.OperationId,
                    SourceSystem = EventSourceSystem,
                    SubjectSystem = EventSourceSystem
                });

        private async Task PublishEventAsync(EventGridEvent eventGridEvent)
        {
            await PublishEventsAsync(new[] { eventGridEvent });
        }

        private async Task PublishEventsAsync(IEnumerable<EventGridEvent> events)
        {
            await _eventGridClient.PublishEventsAsync(
                _settings.TopicHostname,
                events.ToList());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _eventGridClient?.Dispose();
            }
        }
    }
}
