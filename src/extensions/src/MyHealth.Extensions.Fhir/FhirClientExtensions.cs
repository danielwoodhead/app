using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace MyHealth.Extensions.Fhir
{
    public static class FhirClientExtensions
    {
        public static async System.Threading.Tasks.Task EnsurePatientDeviceAsync(this IFhirClient client, string userId)
        {
            var patient = await client.EnsureResourceByIdentifierAsync<Patient>(
                userId,
                FhirConstants.MyHealthSystem,
                (patient, identifier) =>
                {
                    patient.Id = userId;
                    patient.Identifier = new List<Identifier> { identifier };
                });

            await client.EnsureResourceByIdentifierAsync<Device>(
                userId,
                FhirConstants.MyHealthSystem,
                (device, identifier) =>
                {
                    device.Id = userId;
                    device.Identifier = new List<Identifier> { identifier };
                    device.Patient = patient.ToReference();
                });
        }

        public static async System.Threading.Tasks.Task<TResource> EnsureResourceByIdentifierAsync<TResource>(this IFhirClient client, string value, string system, Action<TResource, Identifier> propertySetter = null)
            where TResource : Resource, new()
        {
            Identifier identifier = BuildIdentifier(value, system);

            return await client.GetResourceByIdentifierAsync<TResource>(identifier)
                ?? await client.CreateResourceByIdentifierAsync<TResource>(identifier, propertySetter);
        }

        public static async System.Threading.Tasks.Task<TResource> GetResourceByIdentifierAsync<TResource>(this IFhirClient client, Identifier identifier)
            where TResource : Resource, new()
        {
            SearchParams searchParams = identifier.ToSearchParams();

            Bundle result = await client.SearchAsync<TResource>(searchParams);

            return result.Entry.Select(e => e.Resource).OfType<TResource>().FirstOrDefault();
        }

        public static async System.Threading.Tasks.Task<TResource> CreateResourceByIdentifierAsync<TResource>(this IFhirClient client, Identifier identifier, Action<TResource, Identifier> propertySetter = null)
            where TResource : Resource, new()
        {
            var resource = new TResource();

            propertySetter?.Invoke(resource, identifier);

            // use Update to have control of the resource ID
            return await client.UpdateAsync(resource);
        }

        private static Identifier BuildIdentifier(string value, string system)
        {
            return new Identifier { Value = value, System = system };
        }
    }
}
