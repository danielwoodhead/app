using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Events.Azure.EventGrid;
using MyHealth.Integrations.Core;
using MyHealth.Integrations.Core.Repository;
using MyHealth.Integrations.Fitbit;
using MyHealth.Integrations.Fitbit.Controllers;
using MyHealth.Integrations.Repository.TableStorage;
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

            services.AddControllers()
                .AddApplicationPart(typeof(FitbitController).Assembly);

            services.AddVersioning();
            services.AddHealthChecks();

            services.AddVersionAwareSwagger(options =>
            {
                options.ApiName = "MyHealth Integrations API";
            });

            // prevent mapping of 'sub' claim
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["Authentication:Authority"];
                    options.Audience = Configuration["Authentication:Audience"];
                });

            services.Configure<TableStorageSettings>(Configuration.GetSection("TableStorage"));
            services.AddTransient<IIntegrationsService, IntegrationsService>();
            services.AddSingleton<IIntegrationsRepository, TableStorageIntegrationsRepository>();
            services.AddSingleton<IOperationContext, OperationContext>();
            services.AddScoped<IUserOperationContext, UserOperationContext>();

            services.Configure<EventGridSettings>(Configuration.GetSection("EventGrid"));
            services.AddSingleton<IEventPublisher, EventGridEventPublisher>();

            // providers
            services.Configure<FitbitSettings>(Configuration.GetSection("Fitbit"));
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
