using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Models.Events;

namespace MyHealth.Integrations.Core.Events
{
    public class EventHandler : IEventHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _eventTypeMapping;

        public EventHandler(IServiceProvider serviceProvider)
        {
            _eventTypeMapping = new Dictionary<string, Type>()
            {
                { EventTypes.IntegrationProviderUpdate, typeof(IIntegrationProviderUpdateEventHandler) }
            };

            _serviceProvider = serviceProvider;
        }

        public async Task ProcessAsync(IEvent @event)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (!_eventTypeMapping.TryGetValue(@event.EventType, out var eventHandlerType))
            {
                throw new InvalidOperationException($"No event handler interfaces are defined for event type {@event.EventType}");
            }

            var data = (IntegrationEventData)@event.Data;

            var handlers = ((IEnumerable<IIntegrationEventHandler>)_serviceProvider.GetServices(eventHandlerType)).Where(x => x.Provider == data.Provider);

            if (handlers is null || !handlers.Any())
            {
                throw new InvalidOperationException($"No event handler could be found for provider: {data.Provider} for event type {eventHandlerType.Name}");
            }

            foreach (IIntegrationEventHandler handler in handlers)
            {
                await handler.RunAsync(@event);
            }
        }
    }
}
