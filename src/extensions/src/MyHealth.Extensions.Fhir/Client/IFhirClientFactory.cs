using Hl7.Fhir.Rest;

namespace MyHealth.Extensions.Fhir.Client
{
    internal interface IFhirClientFactory
    {
        IFhirClient Create();
    }
}
