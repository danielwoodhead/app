using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Options;

namespace MyHealth.Extensions.Events.Azure.EventGrid
{
    public class EventGridEventPublisher : IEventPublisher, IDisposable
    {
        private readonly EventGridSettings _settings;
        private readonly EventGridClient _eventGridClient;

        public EventGridEventPublisher(IOptions<EventGridSettings> settings)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _eventGridClient = new EventGridClient(new TopicCredentials(_settings.TopicKey));
        }

        public async Task PublishAsync(DomainEvent e)
        {
            if (!_settings.Enabled)
                return;

            if (e == null)
                throw new ArgumentNullException(nameof(e));

            await PublishEventAsync(e.ToEventGridEvent());
        }

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
