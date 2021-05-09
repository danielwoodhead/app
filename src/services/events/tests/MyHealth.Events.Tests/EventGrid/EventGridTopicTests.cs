using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using CloudNative.CloudEvents;
using Moq;
using MyHealth.Events.Azure.EventGrid.Topics;
using Xunit;

namespace MyHealth.Events.Tests.EventGrid
{
    public class EventGridTopicTests
    {
        private readonly Mock<EventGridPublisherClient> _mockEventGridClient = new();

        [Fact]
        public async Task PublishEvent_PublishesCorrectly()
        {
            EventGridTopic sut = new(_mockEventGridClient.Object);

            var eventId = Guid.NewGuid().ToString();
            var @event = new CloudEvent(CloudEventsSpecVersion.V1_0)
            {
                Id = eventId,
                Type = "test",
                Source = new Uri("http://test.com")
            };

            await sut.PublishEventAsync(@event);

            _mockEventGridClient
                .Verify(x => x.SendEventAsync(
                    It.Is<global::Azure.Messaging.CloudEvent>(e => e.Id == eventId ),
                    It.IsAny<CancellationToken>()));
        }
    }
}
