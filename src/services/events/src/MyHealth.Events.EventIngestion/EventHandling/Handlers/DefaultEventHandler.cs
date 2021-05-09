using System.Threading.Tasks;
using CloudNative.CloudEvents;
using MyHealth.Events.EventIngestion.Repositories;

namespace MyHealth.Events.EventIngestion.EventHandling.Handlers
{
    public class DefaultEventHandler : IEventHandler
    {
        private readonly IEventRepository _repository;

        public DefaultEventHandler(IEventRepository repository)
        {
            _repository = repository;
        }

        public string EventType => EventTypes.Default;

        public async Task HandleEventAsync(CloudEvent @event)
        {
            await _repository.StoreAsync(@event);
        }
    }
}
