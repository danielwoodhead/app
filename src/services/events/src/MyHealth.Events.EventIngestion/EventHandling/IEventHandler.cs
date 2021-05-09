using System.Threading.Tasks;
using CloudNative.CloudEvents;

namespace MyHealth.Events.EventIngestion.EventHandling
{
    public interface IEventHandler
    {
        string EventType { get; }
        Task HandleEventAsync(CloudEvent @event);
    }
}
