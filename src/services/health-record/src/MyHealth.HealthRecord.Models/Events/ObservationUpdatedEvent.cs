using System;
using MyHealth.HealthRecord.Models.Events.Base;

namespace MyHealth.HealthRecord.Models.Events
{
    public class ObservationUpdatedEvent : DomainEvent
    {
        public ObservationUpdatedEvent(string id, string subject, DateTime eventTime, string dataVersion, ObservationEventData data)
            : base(id, subject, eventTime, dataVersion, data)
        {
        }

        public override string EventType => EventTypes.ObservationUpdated;
    }
}
