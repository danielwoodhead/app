using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyHealth.App.Api.Core.Authentication;
using MyHealth.App.Api.Core.Settings;
using MyHealth.App.Api.Extensions;
using MyHealth.App.Api.HealthRecord;
using MyHealth.App.Api.Identity;
using MyHealth.App.Api.Integrations;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.AspNetCore.Versioning;

namespace MyHealth.App.Api
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
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddDistributedMemoryCache();
            services.AddHealthChecks();
            services.AddSwagger(Configuration);
            services.AddVersioning();

            services.Configure<MyHealthAppApiSettings>(Configuration.GetSection("MyHealthAppApi"));
            services.AddTransient<DelegationAuthenticationHandler>();

            // apis
            services.AddHealthRecordApi(Configuration);
            services.AddIdentityApi(Configuration);
            services.AddIntegrationsApi(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(Configuration.GetSection("Cors").GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>());
            });

            app.UseHttpsRedirection();

            app.UseMyHealthSwagger(options =>
            {
                options.OAuthClientId = Configuration["Swagger:OAuthClientId"];
                options.OAuthAppName = "MyHealth App API";
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
