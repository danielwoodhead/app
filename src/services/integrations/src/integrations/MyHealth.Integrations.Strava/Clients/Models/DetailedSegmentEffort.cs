using System;
using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class DetailedSegmentEffort
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

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("activity")]
        public MetaActivity Activity { get; set; }

        [JsonPropertyName("athlete")]
        public MetaAthlete Athlete { get; set; }

        [JsonPropertyName("moving_time")]
        public int? MovingTime { get; set; }

        [JsonPropertyName("start_index")]
        public int? StartIndex { get; set; }

        [JsonPropertyName("end_index")]
        public int? EndIndex { get; set; }

        [JsonPropertyName("average_cadence")]
        public float? AverageCadence { get; set; }

        [JsonPropertyName("average_watts")]
        public float? AverageWatts { get; set; }

        [JsonPropertyName("device_watts")]
        public bool? DeviceWatts { get; set; }

        [JsonPropertyName("average_heartrate")]
        public float? AverageHeartrate { get; set; }

        [JsonPropertyName("max_heartrate")]
        public float? MaxHeartrate { get; set; }

        [JsonPropertyName("segment")]
        public SummarySegment Segment { get; set; }

        [JsonPropertyName("kom_rank")]
        public int? KomRank { get; set; }

        [JsonPropertyName("pr_rank")]
        public int? PrRank { get; set; }

        [JsonPropertyName("hidden")]
        public bool? Hidden { get; set; }
    }
}
