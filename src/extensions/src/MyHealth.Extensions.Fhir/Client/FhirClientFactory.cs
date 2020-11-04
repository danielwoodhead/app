using Hl7.Fhir.Rest;
using Microsoft.Extensions.Options;
using MyHealth.Extensions.Fhir.Authentication;

namespace MyHealth.Extensions.Fhir.Client
{
    internal class FhirClientFactory : IFhirClientFactory
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly FhirClientSettings _fhirClientSettings;

        public FhirClientFactory(
            IAuthenticationService authenticationService,
            IOptions<FhirClientSettings> fhirClientSettings)
        {
            _authenticationService = authenticationService;
            _fhirClientSettings = fhirClientSettings.Value;
        }

        public IFhirClient Create()
        {
            var client = new FhirClient(_fhirClientSettings.BaseUrl)
            {
                PreferredFormat = ResourceFormat.Json,
            };

            client.OnBeforeRequest += (sender, e) =>
            {
                string token = _authenticationService.GetAccessTokenAsync()
                    .GetAwaiter()
                    .GetResult();
                e.RawRequest.Headers.Add("Authorization", $"Bearer {token}");
            };

            return client;
        }
    }
}
