using System.Threading.Tasks;
using Hl7.Fhir.Rest;

namespace MyHealth.HealthRecord.Data.Fhir.Base
{
    public interface IFhirClientFactory
    {
        Task<IFhirClient> InstanceAsync();
    }
}
