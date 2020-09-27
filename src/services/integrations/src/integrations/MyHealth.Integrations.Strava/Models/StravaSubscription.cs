using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Models
{
    public class StravaSubscription
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
