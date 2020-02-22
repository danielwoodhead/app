using System;
using Hl7.Fhir.Model;

namespace MyHealth.Observations.Integration.Fhir.Base
{
    public static class ResourceReferenceExtensions
    {
        public static ResourceReference ForPatient(this ResourceReference resourceReference, string patientId)
        {
            if (resourceReference == null)
                throw new ArgumentNullException(nameof(resourceReference));

            resourceReference.Reference = $"Patient?identifier={IdentifierSystem.Patient}|{patientId}";

            return resourceReference;
        }
    }
}
