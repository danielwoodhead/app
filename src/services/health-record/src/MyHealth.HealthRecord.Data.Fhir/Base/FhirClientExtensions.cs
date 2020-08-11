using System;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace MyHealth.HealthRecord.Data.Fhir.Base
{
    public static class FhirClientExtensions
    {
        public static TransactionBuilder BuildTransaction(this IFhirClient client, Bundle.BundleType bundleType = Bundle.BundleType.Transaction)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return new TransactionBuilder(client.Endpoint.AbsoluteUri, bundleType);
        }

        public static async Task<Patient> EnsurePatientAsync(this IFhirClient client, string userId)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            Bundle bundle = await client.SearchAsync<Patient>(new[] { $"Patient?identifier={IdentifierSystem.Patient}|{userId}" });
            Bundle.EntryComponent bundleEntry = bundle.Entry.SingleOrDefault();

            if (bundleEntry == null)
            {
                var newPatient = new Patient();
                newPatient.Identifier.Add(new Identifier(IdentifierSystem.Patient, userId));
                return await client.CreateAsync(newPatient);
            }

            return (Patient)bundleEntry.Resource;
        }
    }
}
