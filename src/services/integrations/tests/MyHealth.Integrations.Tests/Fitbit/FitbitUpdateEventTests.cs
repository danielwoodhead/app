using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Logging.Abstractions;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Core.IoMT;
using MyHealth.Integrations.Core.IoMT.Models;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Fitbit.EventHandlers;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Fitbit.Services;
using MyHealth.Integrations.FunctionApp.Functions;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Tests.Factories;
using MyHealth.Integrations.Tests.Mocks;
using Xunit;

namespace MyHealth.Integrations.Tests.Fitbit
{
    public class FitbitUpdateEventTests
    {
        [Fact]
        public async Task FitbitUpdateEvent_BodyWeight_PublishedToIoMT()
        {
            var mockIntegrationRepository = new MockIntegrationRepository();
            var mockFitbitClient = new MockFitbitClient();
            var fitbitAuthenticationClient = new MockFitbitAuthenticationClient();
            var mockFhirClient = new MockFhirClient();
            var mockIoMTPublisher = new MockIoMTDataPublisher();
            var mockDateTimeProvider = new MockDateTimeProvider();

            IntegrationEventProcessor sut = CreateSut(
                mockIntegrationRepository,
                mockFitbitClient,
                fitbitAuthenticationClient,
                mockFhirClient,
                mockIoMTPublisher,
                mockDateTimeProvider);

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

            await sut.Run(EventFactory.FitbitProviderUpdateEvent);

            var bodyWeight = Assert.IsType<BodyWeight>(mockIoMTPublisher.Published);
            Assert.Equal(100, bodyWeight.Weight);
        }

        private IntegrationEventProcessor CreateSut(
            IIntegrationRepository integrationRepository,
            IFitbitClient fitbitClient,
            IFitbitAuthenticationClient fitbitAuthenticationClient,
            IFhirClient fhirClient,
            IIoMTDataPublisher ioMTDataPublisher,
            IDateTimeProvider dateTimeProvider)
        {
            return new IntegrationEventProcessor(
                new Core.Events.EventHandler(
                    new[]
                    {
                        new FitbitProviderUpdateEventHandler(
                            fitbitClient,
                            new FitbitTokenService(
                                integrationRepository,
                                dateTimeProvider,
                                fitbitAuthenticationClient),
                            ioMTDataPublisher,
                            fhirClient,
                            NullLogger<FitbitProviderUpdateEventHandler>.Instance)
                    },
                    NullLogger<Core.Events.EventHandler>.Instance));
        }
    }
}
