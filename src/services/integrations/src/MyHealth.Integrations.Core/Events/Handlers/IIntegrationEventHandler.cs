using System.Threading.Tasks;
using MyHealth.Extensions.Events;

namespace MyHealth.Integrations.Core.Events.Handlers
{
    public interface IIntegrationEventHandler
    {
        string Provider { get; }
        Task RunAsync(IEvent e);
    }
}
