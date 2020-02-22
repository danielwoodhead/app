using System;

namespace MyHealth.Observations.Integration.Fhir
{
    public class FhirServerSettings
    {
        public string BaseUrl { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}
