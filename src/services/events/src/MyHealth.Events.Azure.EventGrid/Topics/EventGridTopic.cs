using System;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using MyHealth.Events.EventIngestion.Topics; 

namespace MyHealth.Events.Azure.EventGrid.Topics
{
    public class EventGridTopic : ITopic
    {
        private readonly EventGridPublisherClient _client;

        public EventGridTopic(EventGridPublisherClient client)
        {
            _client = client;
        }

        public async Task PublishEventAsync(CloudEvent @event)
        {
            byte[] encodedEvent = new JsonEventFormatter().EncodeStructuredModeMessage(@event, out _);

            var azureCloudEvent = global::Azure.Messaging.CloudEvent.Parse(new BinaryData(encodedEvent));

            await _client.SendEventAsync(azureCloudEvent);
        }
    }
}
