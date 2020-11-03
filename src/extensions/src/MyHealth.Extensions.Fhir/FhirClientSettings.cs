namespace MyHealth.Extensions.Fhir
{
    public class FhirClientSettings
    {
        public string BaseUrl { get; set; }
        public string AuthenticationTokenEndpoint { get; set; }
        public string AuthenticationClientId { get; set; }
        public string AuthenticationClientSecret { get; set; }
        public string AuthenticationScope { get; set; }
    }
}
