using MyHealth.Events.EventIngestion.Repositories;
using MyHealth.Events.EventIngestion.Topics;

namespace MyHealth.Events.EventIngestion.EventHandling.Handlers
{
    public class TestEventHandler : PublishingEventHandler
    {
        public TestEventHandler(IEventRepository eventRepository, ITopicFactory topicFactory)
            : base(eventRepository, topicFactory)
        {
        }

        public override string EventType => "test";
        public override string TopicName => "test";
    }
}
