using System;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Strava.Models
{
    public class StravaIntegrationData : IProviderData
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresUtc { get; set; }
        public string RefreshToken { get; set; }
    }
}
