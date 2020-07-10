using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyHealth.Extensions.Events
{
    public class CompositeEventPublisher : IEventPublisher
    {
        private readonly IEnumerable<IEventPublisher> _eventPublishers;

        public CompositeEventPublisher(IEnumerable<IEventPublisher> eventPublishers)
        {
            _eventPublishers = eventPublishers ?? throw new ArgumentNullException(nameof(eventPublishers));
        }

        public async Task PublishAsync(IEvent @event)
        {
            foreach (IEventPublisher eventPublisher in _eventPublishers)
            {
                await eventPublisher.PublishAsync(@event);
            }
        }

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            foreach (IEventPublisher eventPublisher in _eventPublishers)
            {
                await eventPublisher.PublishAsync(events);
            }
        }
    }
}
