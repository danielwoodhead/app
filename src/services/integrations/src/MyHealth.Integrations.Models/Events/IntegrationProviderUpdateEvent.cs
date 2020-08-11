using System;
using MyHealth.Extensions.Events;

namespace MyHealth.Integrations.Models.Events
{
    public class IntegrationProviderUpdateEvent : DomainEvent<IntegrationProviderEventData>
    {
        public IntegrationProviderUpdateEvent(string id, string subject, DateTime eventTime, string dataVersion, IntegrationProviderEventData data)
            : base(id, subject, eventTime, dataVersion, data)
        {
        }

        public override string EventType => EventTypes.IntegrationProviderUpdate;
    }
}
