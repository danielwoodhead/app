using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace MyHealth.Observations.Integration.Fhir.Base
{
    public static class FhirClientExtensions
    {
        public static TransactionBuilder BuildTransaction(this IFhirClient client, Bundle.BundleType bundleType = Bundle.BundleType.Transaction)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return new TransactionBuilder(client.Endpoint.AbsoluteUri, bundleType);
        }
    }
}
