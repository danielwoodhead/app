using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHealth.Extensions.AspNetCore.Context;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Integrations.Api.Extensions;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Data.TableStorage;
using MyHealth.Integrations.Fitbit;
using MyHealth.Integrations.Utility;

namespace MyHealth.Integrations.Api
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
            services.AddApplicationInsightsTelemetry();
            services.AddAuthentication(Configuration);
            services.AddContext();
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddEvents(Configuration);
            services.AddHealthChecks();
            services.AddSwagger(Configuration);
            services.AddTableStorage(Configuration);
            services.AddUtility();
            services.AddVersioning();
            services.AddTransient<IIntegrationService, IntegrationService>();

            // providers
            services.AddFitBitCore(Configuration);
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
                options.OAuthAppName = "MyHealth Integrations API";
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}
