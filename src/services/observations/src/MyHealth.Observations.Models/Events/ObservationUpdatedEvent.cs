using System;
using MyHealth.Observations.Models.Events.Base;

namespace MyHealth.Observations.Models.Events
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
