using System.Threading.Tasks;
using MyHealth.Observations.Models.Events.Base;

namespace MyHealth.Observations.Core.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(DomainEvent e);
    }
}
