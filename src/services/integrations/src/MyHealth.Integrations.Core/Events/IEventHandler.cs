using System.Threading.Tasks;
using MyHealth.Extensions.Events;

namespace MyHealth.Integrations.Core.Events
{
    public interface IEventHandler
    {
        Task RunAsync(IEvent @event);
    }
}
