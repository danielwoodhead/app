using System;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Fitbit.Models
{
    public class FitbitIntegrationData : IProviderData
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresUtc { get; set; }
        public string RefreshToken { get; set; }
    }
}
