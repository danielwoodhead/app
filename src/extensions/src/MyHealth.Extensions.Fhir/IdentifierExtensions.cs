using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace MyHealth.Extensions.Fhir
{
    public static class IdentifierExtensions
    {
        public static SearchParams ToSearchParams(this Identifier identifier)
        {
            var searchParams = new SearchParams();
            searchParams.Add("identifier", identifier.ToSearchToken());

            return searchParams;
        }

        public static string ToSearchToken(this Identifier identifier)
        {
            var token = string.Empty;
            if (!string.IsNullOrEmpty(identifier.System))
            {
                token += $"{identifier.System}|";
            }

            token += identifier.Value;
            return token;
        }
    }
}
