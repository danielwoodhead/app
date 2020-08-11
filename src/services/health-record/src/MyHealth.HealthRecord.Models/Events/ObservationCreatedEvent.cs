using System;
using MyHealth.HealthRecord.Models.Events.Base;

namespace MyHealth.HealthRecord.Models.Events
{
    public class ObservationCreatedEvent : DomainEvent
    {
        public ObservationCreatedEvent(string id, string subject, DateTime eventTime, string dataVersion, ObservationEventData data)
            : base(id, subject, eventTime, dataVersion, data)
        {
        }

        public override string EventType => EventTypes.ObservationCreated;
    }
}
