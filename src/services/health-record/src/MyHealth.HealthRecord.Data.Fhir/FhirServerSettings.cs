using System;

namespace MyHealth.HealthRecord.Data.Fhir
{
    public class FhirServerSettings
    {
        public string BaseAddress { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}
