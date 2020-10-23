using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class MetaActivity
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }
    }
}
