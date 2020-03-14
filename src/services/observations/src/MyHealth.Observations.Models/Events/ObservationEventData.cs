using MyHealth.Observations.Models.Events.Base;

namespace MyHealth.Observations.Models.Events
{
    public class ObservationEventData : EventData
    {
        public string UserId { get; set; }
    }
}
