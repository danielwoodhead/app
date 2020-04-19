using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace MyHealth.Extensions.AspNetCore.Swagger
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseVersionAwareSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty; // serve Swagger UI at app route
                var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            return app;
        }
    }
}
