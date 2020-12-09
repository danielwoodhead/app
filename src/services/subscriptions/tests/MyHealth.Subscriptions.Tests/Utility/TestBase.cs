using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Subscriptions.Api;
using Xunit;

namespace MyHealth.Subscriptions.Tests.Utility
{
    public abstract class TestBase : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        protected TestBase(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        protected HttpClient CreateClient(Action<IServiceCollection> configureServices = null)
        {
            HttpClient client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                                "Test", options => { });

                        services.Configure<SwaggerOptions>(s =>
                        {
                            s.AuthorizationUrl = "https://test/auth";
                            s.TokenUrl = "https://test/auth";
                        });

                        configureServices?.Invoke(services);
                    });
                })
                .CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            return client;
        }
    }
}
