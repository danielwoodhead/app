using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Subscriptions.Api.Extensions;
using MyHealth.Subscriptions.Core.Webhooks;
using MyHealth.Subscriptions.TableStorage;

namespace MyHealth.Subscriptions.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(Configuration);
            services.AddControllers();
            services.AddSwagger(Configuration);
            services.AddVersioning();

            services.AddTransient<ISubscriptionWebhookService, SubscriptionWebhookService>();
            services.AddTransient<ISubscriptionWebhookRepository, TableStorageSubscriptionWebhookRepository>();
            services.AddHttpClient<ISubscriptionWebhookClient, SubscriptionWebhookClient>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(10);
            });
            services.AddSingleton<IRandomStringGenerator, GuidRandomStringGenerator>();
            services.AddScoped<IOperationContext, Core.Webhooks.OperationContext>();

            services.AddSingleton(CloudStorageAccount.Parse("UseDevelopmentStorage=true").CreateCloudTableClient());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseMyHealthSwagger(options =>
            {
                options.OAuthClientId = Configuration["Swagger:OAuthClientId"];
                options.OAuthAppName = "MyHealth Subscriptions API";
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("Subscriptions");
            });
        }
    }
}
