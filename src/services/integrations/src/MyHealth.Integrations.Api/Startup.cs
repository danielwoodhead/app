using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Integrations.Api.Extensions;
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
            services.AddAuth(Configuration);
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddHealthChecks();
            services.AddIntegrationsCore(Configuration);
            services.AddTableStorage(Configuration);
            services.AddSwagger();
            services.AddUtility();
            services.AddVersioning();

            // providers
            services.AddFitBitCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Standard ASP.NET Core pattern")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            }

            app.UseHttpsRedirection();

            if (!env.IsProduction())
            {
                app.UseVersionAwareSwagger();
            }

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
