using System.Threading.Tasks;
using CloudNative.CloudEvents;

namespace MyHealth.Events.EventIngestion.Topics
{
    public interface ITopic
    {
        Task PublishEventAsync(CloudEvent @event);
    }
}
