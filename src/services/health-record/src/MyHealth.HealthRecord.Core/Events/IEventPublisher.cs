using System.Threading.Tasks;
using MyHealth.HealthRecord.Models.Events.Base;

namespace MyHealth.HealthRecord.Core.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(DomainEvent e);
    }
}
