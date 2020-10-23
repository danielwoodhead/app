using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class PolylineMap
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("polyline")]
        public string Polyline { get; set; }

        [JsonPropertyName("summary_polyline")]
        public string SummaryPolyline { get; set; }
    }
}
