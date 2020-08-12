using MyHealth.Extensions.Events;

namespace MyHealth.HealthRecord.Models.Events
{
    public class ObservationEventData : EventData
    {
        public string UserId { get; set; }
    }
}
