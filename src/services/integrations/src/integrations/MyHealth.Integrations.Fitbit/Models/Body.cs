using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Fitbit.Models
{
    public class Body
    {
        [JsonPropertyName("bmi")]
        public double Bmi { get; set; }

        [JsonPropertyName("fat")]
        public double Fat { get; set; }

        [JsonPropertyName("weight")]
        public double Weight { get; set; }
    }
}
