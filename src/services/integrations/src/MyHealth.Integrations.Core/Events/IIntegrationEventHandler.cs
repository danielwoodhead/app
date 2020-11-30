using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Core.Events.Handlers
{
    public interface IIntegrationEventHandler : IEventHandler
    {
        string EventType { get; }
        Provider Provider { get; }
    }
}
