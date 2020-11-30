using System.Threading.Tasks;
using MyHealth.Integrations.Core.IoMT;
using MyHealth.Integrations.Core.IoMT.Models;

namespace MyHealth.Integrations.Tests.Mocks
{
    public class MockIoMTDataPublisher : IIoMTDataPublisher
    {
        public IoMTModel Published { get; private set; }

        public Task PublishAsync(IoMTModel model)
        {
            Published = model;
            return Task.CompletedTask;
        }
    }
}
