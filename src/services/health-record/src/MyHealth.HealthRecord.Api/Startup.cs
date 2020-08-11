using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHealth.HealthRecord.Api.Extensions;
using MyHealth.HealthRecord.Api.Middleware;
using MyHealth.HealthRecord.Api.Swagger;
using MyHealth.HealthRecord.Core;
using MyHealth.HealthRecord.Core.Events;
using MyHealth.HealthRecord.Core.Repository;
using MyHealth.HealthRecord.Events.ApplicationInsights;
using MyHealth.HealthRecord.Events.EventGrid;
using MyHealth.HealthRecord.Data.Fhir;
using MyHealth.HealthRecord.Data.Fhir.Base;
using MyHealth.HealthRecord.Utility;

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

            services.Configure<FhirServerSettings>(Configuration.GetSection("FhirServerSettings"));
            services.AddTransient<IFhirClientFactory, FhirClientFactory>();
            services.AddTransient<IObservationsRepository, FhirObservationsRepository>();

            services.Configure<EventGridSettings>(Configuration.GetSection("EventGridSettings"));
            if (Configuration.GetSection("EventGridSettings").GetValue("Enabled", defaultValue: false))
                services.AddSingleton<IEventPublisher, EventGridEventPublisher>();

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<OperationContextMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}
