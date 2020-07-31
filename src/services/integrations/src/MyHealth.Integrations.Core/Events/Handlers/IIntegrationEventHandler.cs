using System.Threading.Tasks;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Core.Events.Handlers
{
    public interface IIntegrationEventHandler
    {
        Provider Provider { get; }
        Task RunAsync(IEvent e);
    }
}
