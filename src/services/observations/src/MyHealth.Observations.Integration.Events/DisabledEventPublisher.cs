using System.Threading.Tasks;
using MyHealth.Observations.Core.Events;
using MyHealth.Observations.Models;

namespace MyHealth.Observations.Integration.Events
{
    public class DisabledEventPublisher : IEventPublisher
    {
        public Task PublishObservationCreatedEvent(Observation observation)
        {
            return Task.CompletedTask;
        }
    }
}
