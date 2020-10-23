using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class SummarySegment
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("activity_type")]
        public string ActivityType { get; set; }

        [JsonPropertyName("distance")]
        public float? Distance { get; set; }

        [JsonPropertyName("average_grade")]
        public float? AverageGrade { get; set; }

        [JsonPropertyName("maximum_grade")]
        public float? MaximumGrade { get; set; }

        [JsonPropertyName("elevation_high")]
        public float? ElevationHigh { get; set; }

        [JsonPropertyName("elevation_low")]
        public float? ElevationLow { get; set; }

        [JsonPropertyName("start_latlng")]
        public float[] StartLatlng { get; set; }

        [JsonPropertyName("end_latlng")]
        public float[] EndLatlng { get; set; }

        [JsonPropertyName("climb_category")]
        public int? ClimbCategory { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("private")]
        public bool? _Private { get; set; }

        [JsonPropertyName("athlete_pr_effort")]
        public SummarySegmentEffort AthletePrEffort { get; set; }
    }
}
