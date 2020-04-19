using MyHealth.Extensions.Events;

namespace MyHealth.Integrations.Models.Events
{
    public class IntegrationEventData : EventData
    {
        public string UserId { get; set; }
    }
}
