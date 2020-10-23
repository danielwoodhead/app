using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class PhotosSummary
    {
        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [JsonPropertyName("primary")]
        public PhotosSummaryPrimary Primary { get; set; }
    }
}
