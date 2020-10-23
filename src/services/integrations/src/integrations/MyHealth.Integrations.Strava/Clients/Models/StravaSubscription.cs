using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class StravaSubscription
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
