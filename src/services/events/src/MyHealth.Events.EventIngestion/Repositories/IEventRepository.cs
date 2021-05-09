using System.Threading.Tasks;
using CloudNative.CloudEvents;

namespace MyHealth.Events.EventIngestion.Repositories
{
    public interface IEventRepository
    {
        Task StoreAsync(CloudEvent @event);
    }
}
