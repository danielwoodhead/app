using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class Split
    {
        [JsonPropertyName("average_speed")]
        public float? AverageSpeed { get; set; }

        [JsonPropertyName("distance")]
        public float? Distance { get; set; }

        [JsonPropertyName("elapsed_time")]
        public int? ElapsedTime { get; set; }

        [JsonPropertyName("elevation_difference")]
        public float? ElevationDifference { get; set; }

        [JsonPropertyName("pace_zone")]
        public int? PaceZone { get; set; }

        [JsonPropertyName("moving_time")]
        public int? MovingTime { get; set; }

        [JsonPropertyName("split")]
        public int? _Split { get; set; }
    }
}
