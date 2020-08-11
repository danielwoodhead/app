using System.Threading.Tasks;
using MyHealth.Integrations.Core.IoMT.Models;

namespace MyHealth.Integrations.Core.IoMT
{
    public interface IIoMTDataPublisher
    {
        Task PublishAsync(IoMTModel model);
    }
}
