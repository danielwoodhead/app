using System.Threading.Tasks;
using MyHealth.Observations.Models;

namespace MyHealth.Observations.Core.Events
{
    public interface IEventPublisher
    {
        Task PublishObservationCreatedEvent(Observation observation);
    }
}
