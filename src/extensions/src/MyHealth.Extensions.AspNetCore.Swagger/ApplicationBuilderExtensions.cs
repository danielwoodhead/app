using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace MyHealth.Extensions.AspNetCore.Swagger
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMyHealthSwagger(this IApplicationBuilder app, Action<SwaggerOptions> configure)
        {
            var options = new SwaggerOptions();
            configure(options);

            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    o.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                o.OAuthClientId(options.OAuthClientId);
                o.OAuthAppName(options.OAuthAppName);
                o.OAuthUsePkce();
            });

            return app;
        }
    }
}
