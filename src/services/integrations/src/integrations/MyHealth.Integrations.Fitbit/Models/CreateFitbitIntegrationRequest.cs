using System;

namespace MyHealth.Integrations.Fitbit.Models
{
    public class CreateFitbitIntegrationRequest
    {
        public string Code { get; set; }
        public Uri RedirectUri { get; set; }
    }
}
