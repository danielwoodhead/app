using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class PhotosSummaryPrimary
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("source")]
        public int? Source { get; set; }

        [JsonPropertyName("unique_id")]
        public string UniqueId { get; set; }

        [JsonPropertyName("urls")]
        public Dictionary<string, string> Urls { get; set; }
    }
}
