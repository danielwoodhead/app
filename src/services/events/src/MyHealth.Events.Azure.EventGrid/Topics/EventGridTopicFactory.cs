using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Azure;
using MyHealth.Events.EventIngestion.Topics;

namespace MyHealth.Events.Azure.EventGrid.Topics
{
    public class EventGridTopicFactory : ITopicFactory
    {
        private readonly IAzureClientFactory<EventGridPublisherClient> _clientFactory;

        public EventGridTopicFactory(IAzureClientFactory<EventGridPublisherClient> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public ITopic Create(string name)
        {
            EventGridPublisherClient client = _clientFactory.CreateClient(name);

            return new EventGridTopic(client);
        }
    }
}
