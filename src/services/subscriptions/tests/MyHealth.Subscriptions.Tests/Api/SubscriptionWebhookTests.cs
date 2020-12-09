using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using MyHealth.Subscriptions.Api;
using MyHealth.Subscriptions.Core.Webhooks;
using MyHealth.Subscriptions.Models;
using MyHealth.Subscriptions.Models.Requests;
using MyHealth.Subscriptions.Tests.Mocks;
using MyHealth.Subscriptions.Tests.Utility;
using Xunit;

namespace MyHealth.Subscriptions.Tests.Api
{
    public class SubscriptionWebhookTests : TestBase
    {
        public SubscriptionWebhookTests(CustomWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task CreateSubscriptionWebhook_InvalidWebhookUrl_ReturnsBadRequest()
        {
            var request = new CreateSubscriptionWebhookRequestModel
            {
                WebhookUrl = "invalidUrl"
            };

            var client = CreateClient();

            var response = await client.PostAsync("/v1.0/subscriptions/webhooks", JsonContent.Create(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            Assert.Equal(ResultCodes.InvalidWebhookUrl, responseContent.Title);
        }

        [Fact]
        public async Task CreateSubscriptionWebhook_WebhookValidationFails_IncorrectStatusCode_ReturnsBadRequest()
        {
            var mockHttpClient = new MockHttpClient();
            mockHttpClient.SetupResponse(HttpStatusCode.NotFound);

            var request = new CreateSubscriptionWebhookRequestModel
            {
                WebhookUrl = "https://mywebhook.com"
            };

            var client = CreateClient(services =>
            {
                services.AddTransient<ISubscriptionWebhookClient>(sp => new SubscriptionWebhookClient(mockHttpClient.Create(), NullLogger<SubscriptionWebhookClient>.Instance));
                services.AddTransient<ISubscriptionWebhookRepository>(sp => new MockSubscriptionWebhookRepository());
            });

            var response = await client.PostAsync("/v1.0/subscriptions/webhooks", JsonContent.Create(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            Assert.Equal(ResultCodes.WebhookValidationFailed, responseContent.Title);
        }

        [Fact]
        public async Task CreateSubscriptionWebhook_WebhookValidationFails_IncorrectResponseFormat_ReturnsBadRequest()
        {
            var mockHttpClient = new MockHttpClient();
            mockHttpClient.SetupResponse(HttpStatusCode.OK, new { foo = "bar" });

            var request = new CreateSubscriptionWebhookRequestModel
            {
                WebhookUrl = "https://mywebhook.com"
            };

            var client = CreateClient(services =>
            {
                services.AddTransient<ISubscriptionWebhookClient>(sp => new SubscriptionWebhookClient(mockHttpClient.Create(), NullLogger<SubscriptionWebhookClient>.Instance));
                services.AddTransient<ISubscriptionWebhookRepository>(sp => new MockSubscriptionWebhookRepository());
            });

            var response = await client.PostAsync("/v1.0/subscriptions/webhooks", JsonContent.Create(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            Assert.Equal(ResultCodes.WebhookValidationFailed, responseContent.Title);
        }

        [Fact]
        public async Task CreateSubscriptionWebhook_WebhookValidationFails_IncorrectVerificationCode_ReturnsBadRequest()
        {
            var mockHttpClient = new MockHttpClient();
            mockHttpClient.SetupResponse(HttpStatusCode.OK, new SubscriptionWebhookValidationResponse { Verify = "incorrectVerificationCode" });

            var request = new CreateSubscriptionWebhookRequestModel
            {
                WebhookUrl = "https://mywebhook.com"
            };

            var client = CreateClient(services =>
            {
                services.AddTransient<ISubscriptionWebhookClient>(sp => new SubscriptionWebhookClient(mockHttpClient.Create(), NullLogger<SubscriptionWebhookClient>.Instance));
                services.AddTransient<ISubscriptionWebhookRepository>(sp => new MockSubscriptionWebhookRepository());
                services.AddSingleton<IRandomStringGenerator>(sp => new MockRandomStringGenerator { Returns = "correctVerificationCode" });
            });

            var response = await client.PostAsync("/v1.0/subscriptions/webhooks", JsonContent.Create(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            Assert.Equal(ResultCodes.WebhookValidationFailed, responseContent.Title);
        }

        [Fact]
        public async Task CreateSubscriptionWebhook_ApplicationAlreadyHasWebhook_ReturnsConflict()
        {
            var request = new CreateSubscriptionWebhookRequestModel
            {
                WebhookUrl = "https://mywebhook.com"
            };

            var client = CreateClient(services =>
            {
                services.AddTransient<ISubscriptionWebhookRepository>(sp => new MockSubscriptionWebhookRepository
                {
                    GetSubscriptionReturnValue = new SubscriptionWebhook()
                });
            });

            var response = await client.PostAsync("/v1.0/subscriptions/webhooks", JsonContent.Create(request));

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

            Assert.Equal(ResultCodes.MaximumWebhooksExceeded, responseContent.Title);
        }

        [Fact]
        public async Task CreateSubscriptionWebhook_Success_ReturnsCreated()
        {
            var mockHttpClient = new MockHttpClient();
            mockHttpClient.SetupResponse(HttpStatusCode.OK, new SubscriptionWebhookValidationResponse { Verify = "correctVerificationCode" });

            var request = new CreateSubscriptionWebhookRequestModel
            {
                WebhookUrl = "https://mywebhook.com"
            };

            var client = CreateClient(services =>
            {
                services.AddTransient<ISubscriptionWebhookClient>(sp => new SubscriptionWebhookClient(mockHttpClient.Create(), NullLogger<SubscriptionWebhookClient>.Instance));
                services.AddTransient<ISubscriptionWebhookRepository>(sp => new MockSubscriptionWebhookRepository
                {
                    GetSubscriptionReturnValue = null,
                    AddSubscriptionReturnValue = new SubscriptionWebhook
                    {
                        Id = Guid.NewGuid().ToString(),
                        WebhookUrl = "https://mywebhook.com"
                    }
                });
                services.AddSingleton<IRandomStringGenerator>(sp => new MockRandomStringGenerator { Returns = "correctVerificationCode" });
            });

            var response = await client.PostAsync("/v1.0/subscriptions/webhooks", JsonContent.Create(request));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadFromJsonAsync<SubscriptionWebhook>();

            Assert.Equal("https://mywebhook.com", responseContent.WebhookUrl);
        }
    }
}
