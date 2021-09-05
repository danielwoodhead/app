using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyHealth.Events.Api;
using MyHealth.Events.EventIngestion.EventHandling;
using MyHealth.Events.EventIngestion.Repositories;
using MyHealth.Events.EventIngestion.Topics;
using MyHealth.Extensions.Testing.Xunit;
using Xunit;

namespace MyHealth.Events.Tests.Api.Events
{
    public class EventsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public EventsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [FileContentData("Api/Events/cloud_event.json")]
        public async Task ValidCloudEvent_ReturnsNoContent(string eventJson)
        {
            HttpClient client = CreateClient();

            HttpResponseMessage response = await PostEventAsync(client, eventJson);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("{}")]
        [FileContentData("Api/Events/cloud_event_missing_id.json")]
        [FileContentData("Api/Events/cloud_event_missing_source.json")]
        [FileContentData("Api/Events/cloud_event_missing_spec_version.json")]
        [FileContentData("Api/Events/cloud_event_missing_type.json")]
        public async Task InvalidCloudEvent_ReturnsBadRequest(string eventJson)
        {
            HttpClient client = CreateClient();

            HttpResponseMessage response = await PostEventAsync(client, eventJson);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // TODO: default handler, event is stored
        // TODO: publishing handler, event is stored & published

        private HttpClient CreateClient() => CreateClient(Enumerable.Empty<IEventHandler>());

        private HttpClient CreateClient(IEnumerable<IEventHandler> eventHandlers)
        {
            return _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        services
                            .AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                        services.AddTransient(s => eventHandlers);
                        services.AddTransient(s => Mock.Of<IEventRepository>());
                        services.AddTransient(s => Mock.Of<ITopicFactory>());
                    }))
                .CreateClient();
        }

        public async Task<HttpResponseMessage> PostEventAsync(HttpClient client, string eventJson)
        {
            return await client.PostAsync("v1/events", new StringContent(eventJson, Encoding.UTF8, MediaTypeNames.Application.CloudEventsJson));
        }
    }
}
