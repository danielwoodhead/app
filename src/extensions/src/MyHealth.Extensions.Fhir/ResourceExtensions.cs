using Hl7.Fhir.Model;

namespace MyHealth.Extensions.Fhir
{
    public static class ResourceExtensions
    {
        public static ResourceReference ToReference<TResource>(this TResource resource)
            where TResource : Resource
        {
            if (resource == null)
            {
                return null;
            }

            return new ResourceReference($@"{typeof(TResource).Name}/{resource.Id}");
        }
    }
}
