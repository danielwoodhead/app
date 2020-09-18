using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHealth.HealthRecord.Core;
using MyHealth.HealthRecord.Data.Fhir;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.HealthRecord.Api.Extensions;
using MyHealth.Extensions.AspNetCore.Context;

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
            services.AddAuthentication(Configuration);
            services.AddContext();
            services.AddControllers();
            services.AddEvents(Configuration);
            services.AddFhir(Configuration);
            services.AddHealthChecks();
            services.AddSwagger(Configuration);
            services.AddVersioning();
            services.AddTransient<IObservationsService, ObservationsService>();
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
                options.OAuthAppName = "MyHealth Health Record API";
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
