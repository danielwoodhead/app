using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Models
{
    public class StravaUpdateNotification
    {
        [JsonPropertyName("aspect_type")]
        public string AspectType { get; set; }

        [JsonPropertyName("event_time")]
        public long EventTime { get; set; } // unix timestamp

        [JsonPropertyName("object_id")]
        public long ObjectId { get; set; }

        [JsonPropertyName("object_type")]
        public string ObjectType { get; set; } // "activity", "athlete"

        [JsonPropertyName("owner_id")]
        public long OwnerId { get; set; }

        [JsonPropertyName("subscription_id")]
        public int SubscriptionId { get; set; }

        [JsonPropertyName("updates")]
        public Dictionary<string, string> Updates { get; set; }
    }
}
