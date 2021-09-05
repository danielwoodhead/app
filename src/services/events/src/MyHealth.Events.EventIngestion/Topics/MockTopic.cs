using System.Threading.Tasks;
using CloudNative.CloudEvents;

namespace MyHealth.Events.EventIngestion.Topics
{
    public class MockTopic : ITopic
    {
        public Task PublishEventAsync(CloudEvent @event) => Task.CompletedTask;
    }
}
