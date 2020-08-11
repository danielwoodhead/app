using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using MyHealth.HealthRecord.Core.Events;
using MyHealth.HealthRecord.Models.Events.Base;

namespace MyHealth.HealthRecord.Events.ApplicationInsights
{
    public class ApplicationInsightsEventPublisher : IEventPublisher
    {
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsEventPublisher(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        public Task PublishAsync(DomainEvent e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            _telemetryClient.TrackEvent(e.EventType, e.Properties);
            return Task.CompletedTask;
        }
    }
}
