using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Fitbit.Models
{
    public class ResourceContainer
    {
        [JsonPropertyName("body")]
        public Body Body { get; set; }
    }
}
