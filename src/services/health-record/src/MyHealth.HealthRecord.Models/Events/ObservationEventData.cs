using MyHealth.HealthRecord.Models.Events.Base;

namespace MyHealth.HealthRecord.Models.Events
{
    public class ObservationEventData : EventData
    {
        public string UserId { get; set; }
    }
}
