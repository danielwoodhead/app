using System;
using MyHealth.Extensions.Events;

namespace MyHealth.HealthRecord.Models.Events
{
    public class ObservationDeletedEvent : DomainEvent
    {
        public ObservationDeletedEvent(string id, string subject, DateTime eventTime, string dataVersion, ObservationEventData data)
            : base(id, subject, eventTime, dataVersion, data)
        {
        }

        public override string EventType => EventTypes.ObservationDeleted;
    }
}
