using System;
using System.Text.Json.Serialization;

namespace MyHealth.Integrations.Core.IoMT.Models
{
    public class BodyWeight : IoMTModel
    {
        [JsonPropertyName("weight")]
        public double Weight { get; set; }

        [JsonPropertyName("measurementDateTime")]
        public DateTime MeasurementDateTime { get; set; }

        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; }

        [JsonPropertyName("patientId")]
        public string PatientId { get; set; }
    }
}
