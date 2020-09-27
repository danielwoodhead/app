using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Models
{
    public class ValidateStravaSubscriptionResponse
    {
        [JsonPropertyName("hub.challenge")]
        public string Challenge { get; set; }
    }
}
