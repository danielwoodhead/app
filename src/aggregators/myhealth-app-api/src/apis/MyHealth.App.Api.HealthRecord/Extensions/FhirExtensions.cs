using System;
using System.Linq;
using FHIR = Hl7.Fhir.Model;

namespace MyHealth.App.Api.HealthRecord.Extensions
{
    public static class FhirExtensions
    {
        public static string ToValue(this FHIR.Observation observation)
        {
            if (observation.Value != null)
            {
                return observation.Value.ToValue();
            }
            else if (observation.Component != null && observation.Component.Any())
            {
                return string.Join(", ", observation.Component.Select(c => c.ToValue()));
            }
            else
            {
                throw new ArgumentException($"Unsupported FHIR observation value type.");
            }
        }

        public static string ToValue(this FHIR.Observation.ComponentComponent component)
        {
            return $"{component.Code.Text}={component.Value.ToValue()}";
        }

        public static string ToValue(this FHIR.Element element)
        {
            switch (element)
            {
                case FHIR.FhirString s:
                    return s.Value;
                case FHIR.Quantity q:
                    return $"{q.Value}{q.Code}";
                default:
                    throw new ArgumentException($"Unsupported FHIR value type {element.GetType().Name}.");
            }
        }
    }
}
