using System;
using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Core.IoMT.Models
{
    public class BikeRide : IoMTModel
    {
        [JsonPropertyName("type")]
        public string Type => "bikeRide";

        [JsonPropertyName("distance")]
        public float Distance { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("measurementDateTime")]
        public DateTime MeasurementDateTime { get; set; }

        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; }

        [JsonPropertyName("patientId")]
        public string PatientId { get; set; }
    }
}
