using System.Threading.Tasks;
using Hl7.Fhir.Rest;

namespace MyHealth.Observations.Integration.Fhir.Base
{
    public interface IFhirClientFactory
    {
        Task<IFhirClient> InstanceAsync();
    }
}
