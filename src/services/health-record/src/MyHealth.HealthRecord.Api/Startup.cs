using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHealth.HealthRecord.Core;
using MyHealth.HealthRecord.Core.Data;
using MyHealth.HealthRecord.Data.Fhir;
using MyHealth.HealthRecord.Data.Fhir.Base;
using MyHealth.HealthRecord.Utility;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.DependencyInjection;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Events.Azure.EventGrid;
using MyHealth.Extensions.Events.ApplicationInsights;
using MyHealth.Extensions.AspNetCore.Versioning;

namespace MyHealth.HealthRecord.Api
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
            services.AddControllers();
            services.AddVersioning();
            services.AddHealthChecks();
            services.AddVersionAwareSwagger(options =>
            {
                options.ApiName = "MyHealth Health Record API";
            });

            // prevent mapping of 'sub' claim
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["Authentication:Authority"];
                    options.Audience = Configuration["Authentication:Audience"];
                });

            services.AddScoped<IOperationContext, OperationContext>();
            services.AddTransient<IObservationsService, ObservationsService>();

            services.Configure<FhirServerSettings>(Configuration.GetSection("FhirServer"));
            services.AddTransient<IFhirClientFactory, FhirClientFactory>();
            services.AddTransient<IObservationsRepository, FhirObservationsRepository>();

            services.Configure<EventGridSettings>(Configuration.GetSection("EventGrid"));
            services.AddSingleton<IEventPublisher, EventGridEventPublisher>();
            services.AddTransient<IEventPublisher, ApplicationInsightsEventPublisher>();
            services.AddComposite<IEventPublisher, CompositeEventPublisher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
