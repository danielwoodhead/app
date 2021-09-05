using System;
using System.Collections.Generic;
using Azure;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Events.Api.Configuration;
using MyHealth.Events.Azure.BlobStorage.Configuration;
using MyHealth.Events.Azure.BlobStorage.Extensions;
using MyHealth.Events.Azure.EventGrid.Extensions;
using MyHealth.Events.EventIngestion.EventHandling;
using MyHealth.Events.EventIngestion.EventHandling.Handlers;
using MyHealth.Events.EventIngestion.Services;
using MyHealth.Extensions.AspNetCore.Swagger;

namespace MyHealth.Events.Api.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddAzureClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(configuration);
                builder.AddAzureEventGrid(configuration);
            });

            return services;
        }

        private static AzureClientFactoryBuilder AddBlobServiceClient(this AzureClientFactoryBuilder builder, IConfiguration configuration)
        {
            var blobStorageSettings = configuration.GetSection("BlobStorage").Get<BlobStorageSettings>();

            builder.AddBlobServiceClient(blobStorageSettings.ConnectionString);

            return builder;
        }

        private static AzureClientFactoryBuilder AddAzureEventGrid(this AzureClientFactoryBuilder builder, IConfiguration configuration)
        {
            var topicSettings = configuration.GetSection("Topics").Get<TopicSettings[]>();

            foreach (TopicSettings topicSetting in topicSettings)
            {
                // TODO: get 'key' from key vault
                builder
                    .AddEventGridPublisherClient(new Uri(topicSetting.Uri), new AzureKeyCredential(topicSetting.Key))
                    .WithName(topicSetting.Name);
            }

            return builder;
        }

        public static IServiceCollection AddEventIngestion(this IServiceCollection services)
        {
            services.AddTransient<IEventService, EventService>();
            services.AddTransient<IEventHandler, DefaultEventHandler>();
            services.AddTransient<IEventHandler, TestEventHandler>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(configuration);
            services.AddAzureBlobStorage(configuration);
            services.AddAzureEventGrid();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMyHealthSwagger(options =>
            {
                options.ApiName = "MyHealth Events API";
                options.AuthorizationScopes = new Dictionary<string, string>
                {
                    { "myhealth-events-api", "MyHealth Events API" }
                };
                options.AuthorizationUrl = configuration["Swagger:AuthorizationUrl"];
                options.TokenUrl = configuration["Swagger:TokenUrl"];
            });

            return services;
        }
    }
}
