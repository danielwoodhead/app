using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Core.IoMT;
using MyHealth.Integrations.Core.IoMT.Models;

namespace MyHealth.Integrations.IoMT.EventHub
{
    public class EventHubIoMTDataPublisher : IIoMTDataPublisher
    {
        private readonly EventHubProducerClient _eventHub;

        public EventHubIoMTDataPublisher(IOptions<EventHubSettings> settings)
        {
            _eventHub = new EventHubProducerClient(settings.Value.ConnectionString, settings.Value.Name);
        }

        public async Task PublishAsync(IoMTModel model)
        {
            await _eventHub.SendAsync(new[]
            {
                new EventData(JsonSerializer.SerializeToUtf8Bytes(model))
            });
        }
    }
}
