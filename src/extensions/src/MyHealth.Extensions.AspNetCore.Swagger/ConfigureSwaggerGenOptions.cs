using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MyHealth.Extensions.AspNetCore.Swagger.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyHealth.Extensions.AspNetCore.Swagger
{
    internal class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly SwaggerOptions _options;

        public ConfigureSwaggerGenOptions(
            IApiVersionDescriptionProvider provider,
            IOptions<SwaggerOptions> options)
        {
            _provider = provider;
            _options = options.Value;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }

            options.OperationFilter<TagByApiExplorerSettingsOperationFilter>();
            options.OperationFilter<AuthorizeCheckOperationFilter>();
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(_options.AuthorizationUrl),
                        TokenUrl = new Uri(_options.TokenUrl),
                        Scopes = _options.AuthorizationScopes
                    }
                }
            });
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = $"{_options.ApiName} {description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
