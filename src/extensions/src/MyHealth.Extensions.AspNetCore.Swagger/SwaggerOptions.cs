using System.Collections.Generic;

namespace MyHealth.Extensions.AspNetCore.Swagger
{
    public class SwaggerOptions
    {
        public string ApiName { get; set; }
        public string AuthorizationUrl { get; set; }
        public string TokenUrl { get; set; }
        public Dictionary<string, string> AuthorizationScopes { get; set; }
        public string OAuthClientId { get; set; }
        public string OAuthAppName { get; set; }
    }
}
