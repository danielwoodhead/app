using System;

namespace MyHealth.Extensions.Fhir
{
    public class FhirClientSettings
    {
        public string BaseUrl { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
        public AuthenticationMode AuthenticationMode { get; set; }
        public string AuthenticationTokenEndpoint { get; set; }
        public string AuthenticationClientId { get; set; }
        public string AuthenticationClientSecret { get; set; }
        public string AuthenticationScope { get; set; }
    }

    public enum AuthenticationMode
    {
        Custom,
        AuthenticatedUser,
        ClientCredentials
    }
}
