using System;

namespace MyHealth.HealthRecord.Data.Fhir
{
    public class FhirServerSettings
    {
        public string BaseUrl { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}
