using System.Threading.Tasks;
using MyHealth.Extensions.Events;

namespace MyHealth.Integrations.Core.Events
{
    public interface IEventHandler
    {
        Task ProcessAsync(IEvent e);
    }
}
