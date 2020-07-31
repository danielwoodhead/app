using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace MyHealth.Extensions.Events.ApplicationInsights
{
    public class ApplicationInsightsEventPublisher : IEventPublisher
    {
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsEventPublisher(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public Task PublishAsync(IEvent @event)
        {
            _telemetryClient.TrackEvent(@event.EventType, @event.Properties);

            return Task.CompletedTask;
        }

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            foreach (IEvent @event in events)
            {
                await PublishAsync(@event).ConfigureAwait(false);
            }
        }
    }
}
