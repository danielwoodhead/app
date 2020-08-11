using System;
using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Fitbit.Models
{
    public class FitbitUpdateNotification
    {
        [JsonPropertyName("collectionType")]
        public string CollectionType { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("ownerId")]
        public string OwnerId { get; set; }

        [JsonPropertyName("ownerType")]
        public string OwnerType { get; set; }

        [JsonPropertyName("subscriptionId")]
        public string SubscriptionId { get; set; }
    }
}
