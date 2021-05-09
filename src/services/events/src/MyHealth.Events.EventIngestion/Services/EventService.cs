using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using MyHealth.Events.EventIngestion.EventHandling;

namespace MyHealth.Events.EventIngestion.Services
{
    public class EventService : IEventService
    {
        private readonly IEnumerable<IEventHandler> _handlers;

        public EventService(IEnumerable<IEventHandler> handlers)
        {
            _handlers = handlers;
        }

        public async Task HandleEventAsync(CloudEvent @event)
        {
            IEventHandler handler = _handlers.SingleOrDefault(handler => handler.EventType == @event.Type);

            if (handler != null)
            {
                await handler.HandleEventAsync(@event);
            }
        }
    }
}
