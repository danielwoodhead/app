using System.Threading.Tasks;

namespace MyHealth.Extensions.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(DomainEvent e);
    }
}
