using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyHealth.Extensions.AspNetCore.Swagger
{
    public class TagByApiExplorerSettingsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var apiExplorerSettings = controllerActionDescriptor
                    .ControllerTypeInfo.GetCustomAttributes(typeof(ApiExplorerSettingsAttribute), true)
                    .Cast<ApiExplorerSettingsAttribute>().FirstOrDefault();

                operation.Tags = new List<OpenApiTag>
                {
                    new OpenApiTag
                    {
                        Name = apiExplorerSettings != null && !string.IsNullOrWhiteSpace(apiExplorerSettings.GroupName)
                            ? apiExplorerSettings.GroupName
                            : controllerActionDescriptor.ControllerName
                    }
                };
            }
        }
    }
}
