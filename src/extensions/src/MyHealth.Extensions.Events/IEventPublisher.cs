using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyHealth.Extensions.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEvent @event);
        Task PublishAsync(IEnumerable<IEvent> events);
    }
}
