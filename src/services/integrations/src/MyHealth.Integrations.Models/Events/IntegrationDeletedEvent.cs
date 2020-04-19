using System;
using MyHealth.Extensions.Events;

namespace MyHealth.Integrations.Models.Events
{
    public class IntegrationDeletedEvent : DomainEvent<IntegrationEventData>
    {
        public IntegrationDeletedEvent(string id, string subject, DateTime eventTime, string dataVersion, IntegrationEventData data)
            : base(id, subject, eventTime, dataVersion, data)
        {
        }

        public override string EventType => EventTypes.IntegrationDeleted;
    }
}
