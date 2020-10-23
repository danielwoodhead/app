using System;
using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class SummarySegmentEffort
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("elapsed_time")]
        public int? ElapsedTime { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("start_date_local")]
        public DateTime? StartDateLocal { get; set; }

        [JsonPropertyName("distance")]
        public float? Distance { get; set; }

        [JsonPropertyName("is_kom")]
        public bool? IsKom { get; set; }
    }
}
