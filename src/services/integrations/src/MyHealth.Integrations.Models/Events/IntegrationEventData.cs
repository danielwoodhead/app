using MyHealth.Extensions.Events;

namespace MyHealth.Integrations.Models.Events
{
    public class IntegrationEventData : EventData
    {
        public string Provider { get; set; }
        public string UserId { get; set; }
    }
}
