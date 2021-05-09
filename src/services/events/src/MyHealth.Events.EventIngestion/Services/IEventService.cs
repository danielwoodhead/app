using System.Threading.Tasks;
using CloudNative.CloudEvents;

namespace MyHealth.Events.EventIngestion.Services
{
    public interface IEventService
    {
        Task HandleEventAsync(CloudEvent @event);
    }
}
