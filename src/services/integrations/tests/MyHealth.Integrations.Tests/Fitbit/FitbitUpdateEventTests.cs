using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Integrations.Api;
using MyHealth.Integrations.Api.Configuration;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Core.IoMT;
using MyHealth.Integrations.Core.IoMT.Models;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Tests.Factories;
using MyHealth.Integrations.Tests.Mocks;
using Newtonsoft.Json;
using Xunit;

namespace MyHealth.Integrations.Tests.Fitbit
{
    public class FitbitUpdateEventTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public FitbitUpdateEventTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task FitbitUpdateEvent_BodyWeight_PublishedToIoMT()
        {
            var mockIntegrationRepository = new MockIntegrationRepository();
            var mockFitbitClient = new MockFitbitClient();
            var fitbitAuthenticationClient = new MockFitbitAuthenticationClient();
            var mockFhirClient = new MockFhirClient();
            var mockIoMTPublisher = new MockIoMTDataPublisher();
            var mockDateTimeProvider = new MockDateTimeProvider();

            mockIntegrationRepository.Integration = new Integration
            {
                Data = new FitbitIntegrationData
                {
                    AccessTokenExpiresUtc = mockDateTimeProvider.UtcNow.AddMinutes(30)
                }
            };

            mockFitbitClient.ResourceToReturn = new ResourceContainer
            {
                Body = new Body
                {
                    Weight = 100
                }
            };

            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.Configure<IntegrationsApiSettings>(settings =>
                        {
                            settings.EventsApiKey = "test";
                        });
                        services.AddTransient<IIntegrationRepository>(s => mockIntegrationRepository);
                        services.AddTransient<IFitbitClient>(s => mockFitbitClient);
                        services.AddTransient<IFitbitAuthenticationClient>(s => fitbitAuthenticationClient);
                        services.AddTransient<IFhirClient>(s => mockFhirClient);
                        services.AddTransient<IIoMTDataPublisher>(s => mockIoMTPublisher);
                        services.AddTransient<IDateTimeProvider>(s => mockDateTimeProvider);
                    });
                })
                .CreateClient();


            var response = await client.PostAsync(
                "/v1/events?apiKey=test",
                new StringContent(
                    JsonConvert.SerializeObject(new[] { EventFactory.FitbitProviderUpdateEvent }),
                    Encoding.UTF8,
                    "application/json"));

            var bodyWeight = Assert.IsType<BodyWeight>(mockIoMTPublisher.Published);
            Assert.Equal(100, bodyWeight.Weight);
        }
    }
}
