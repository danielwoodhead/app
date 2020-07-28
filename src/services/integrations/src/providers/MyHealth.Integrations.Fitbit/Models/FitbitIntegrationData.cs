using System;

namespace MyHealth.Integrations.Fitbit.Models
{
    public class FitbitIntegrationData
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresUtc { get; set; }
        public string RefreshToken { get; set; }
    }
}
