using System;

namespace MyHealth.Integrations.Strava.Models
{
    public class StravaIntegrationData
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresUtc { get; set; }
        public string RefreshToken { get; set; }
    }
}
