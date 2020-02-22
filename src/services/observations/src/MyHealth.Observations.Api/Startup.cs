using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHealth.Observations.Api.Extensions;
using MyHealth.Observations.Api.Swagger;
using MyHealth.Observations.Core;
using MyHealth.Observations.Core.Events;
using MyHealth.Observations.Core.Repository;
using MyHealth.Observations.Integration.Events;
using MyHealth.Observations.Integration.Events.ApplicationInsights;
using MyHealth.Observations.Integration.Events.EventGrid;
using MyHealth.Observations.Integration.Fhir;
using MyHealth.Observations.Utility;

namespace MyHealth.Observations.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Standard ASP.NET Core pattern")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddHealthChecks();
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddVersionAwareSwagger();

            services.AddSingleton<IOperationContext, OperationContext>();
            services.AddTransient<IObservationsService, ObservationsService>();

            services.Configure<FhirServerSettings>(Configuration.GetSection("FhirServerSettings"));
            services.AddTransient<IObservationsRepository, FhirObservationsRepository>();

            services.Configure<EventGridSettings>(Configuration.GetSection("EventGridSettings"));
            if (Configuration.GetSection("EventGridSettings").GetValue("Enabled", defaultValue: false))
                services.AddSingleton<IEventPublisher, EventGridEventPublisher>();
            else
                services.AddSingleton<IEventPublisher, DisabledEventPublisher>();

            services.AddTransient<IEventPublisher, ApplicationInsightsEventPublisher>();
            services.AddComposite<IEventPublisher, CompositeEventPublisher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Standard ASP.NET Core pattern")]
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
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}
