using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace MyHealth.HealthRecord.Data.Fhir.Base
{
    public static class TransactionBuilderExtensions
    {
        public static TransactionBuilder ForPatient(this TransactionBuilder builder, string patientId)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var patient = new Patient();
            patient.Identifier.Add(new Identifier(IdentifierSystem.Patient, patientId));

            var condition = new SearchParams();
            condition.Add("identifier", $"{IdentifierSystem.Patient}|{patientId}");

            return builder.Create(patient, condition);
        }
    }
}
