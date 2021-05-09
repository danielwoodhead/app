using System;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using MyHealth.Events.EventIngestion.Repositories;
using MyHealth.Events.EventIngestion.Topics;

namespace MyHealth.Events.EventIngestion.EventHandling.Handlers
{
    public abstract class PublishingEventHandler : IEventHandler
    {
        private readonly IEventRepository _repository;
        private readonly ITopicFactory _topicFactory;

        protected PublishingEventHandler(IEventRepository repository, ITopicFactory topicFactory)
        {
            _repository = repository;
            _topicFactory = topicFactory;
        }

        public abstract string EventType { get; }
        public abstract string TopicName { get; }

        public virtual async Task HandleEventAsync(CloudEvent @event)
        {
            EventEvaluation evaluation = await EvaluateAsync(@event);

            if (evaluation.ShouldStore)
            {
                await _repository.StoreAsync(@event);
            }

            if (evaluation.ShouldPublish)
            {
                ITopic topic = _topicFactory.Create(TopicName);

                if (topic is null)
                {
                    throw new InvalidOperationException($"Event {@event.Id} could not be published. Topic {TopicName} not found.");
                }

                await topic.PublishEventAsync(evaluation.EventToPublish);
            }
        }

        protected virtual Task<EventEvaluation> EvaluateAsync(CloudEvent @event) =>
            Task.FromResult(new EventEvaluation(
                eventToPublish: @event,
                shouldPublish: true,
                shouldStore: true));

        protected class EventEvaluation
        {
            public CloudEvent EventToPublish { get; }
            public bool ShouldPublish { get; }
            public bool ShouldStore { get; }

            public EventEvaluation(CloudEvent eventToPublish, bool shouldPublish, bool shouldStore)
            {
                EventToPublish = eventToPublish;
                ShouldPublish = shouldPublish;
                ShouldStore = shouldStore;
            }
        }
    }
}
