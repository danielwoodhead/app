using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using MyHealth.Observations.Core.Events;
using MyHealth.Observations.Models.Events.Base;

namespace MyHealth.Observations.Integration.Events.ApplicationInsights
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
