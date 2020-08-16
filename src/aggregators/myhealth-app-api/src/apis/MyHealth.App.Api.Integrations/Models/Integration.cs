using System.Text.Json.Serialization;

namespace MyHealth.App.Api.Integrations.Models
{
    public class Integration
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("provider")]
        public Provider Provider { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Provider
    {
        Fitbit
    }
}
