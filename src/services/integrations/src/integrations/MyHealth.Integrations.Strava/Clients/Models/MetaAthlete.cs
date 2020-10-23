using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Strava.Clients.Models
{
    public class MetaAthlete
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }
}
