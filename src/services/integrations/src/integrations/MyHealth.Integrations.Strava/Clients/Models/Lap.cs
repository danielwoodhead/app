using System;
using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class Lap
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("activity")]
        public MetaActivity Activity { get; set; }

        [JsonPropertyName("athlete")]
        public MetaAthlete Athlete { get; set; }

        [JsonPropertyName("average_cadence")]
        public float? AverageCadence { get; set; }

        [JsonPropertyName("average_speed")]
        public float? AverageSpeed { get; set; }

        [JsonPropertyName("distance")]
        public float? Distance { get; set; }

        [JsonPropertyName("elapsed_time")]
        public int? ElapsedTime { get; set; }

        [JsonPropertyName("start_index")]
        public int? StartIndex { get; set; }

        [JsonPropertyName("end_index")]
        public int? EndIndex { get; set; }

        [JsonPropertyName("lap_index")]
        public int? LapIndex { get; set; }

        [JsonPropertyName("max_speed")]
        public float? MaxSpeed { get; set; }

        [JsonPropertyName("moving_time")]
        public int? MovingTime { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("pace_zone")]
        public int? PaceZone { get; set; }

        [JsonPropertyName("split")]
        public int? Split { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("start_date_local")]
        public DateTime? StartDateLocal { get; set; }

        [JsonPropertyName("total_elevation_gain")]
        public float? TotalElevationGain { get; set; }
    }
}
