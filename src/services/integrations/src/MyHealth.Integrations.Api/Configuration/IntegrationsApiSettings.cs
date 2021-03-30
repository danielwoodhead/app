namespace MyHealth.Integrations.Api.Configuration
{
    public class IntegrationsApiSettings
    {
        public string AuthenticationClientId { get; set; }
        public string AuthenticationClientSecret { get; set; }
        public string AuthenticationScope { get; set; }
        public string AuthenticationTokenEndpoint { get; set; }
        public string EventsApiKey { get; set; }
    }
}
