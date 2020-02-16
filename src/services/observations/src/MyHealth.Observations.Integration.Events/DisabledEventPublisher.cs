using System.Threading.Tasks;
using MyHealth.Observations.Core.Events;
using MyHealth.Observations.Models.Events.Base;

namespace MyHealth.Observations.Integration.Events
{
    public class DisabledEventPublisher : IEventPublisher
    {
        public Task PublishAsync(DomainEvent e)
        {
            return Task.CompletedTask;
        }
    }
}
