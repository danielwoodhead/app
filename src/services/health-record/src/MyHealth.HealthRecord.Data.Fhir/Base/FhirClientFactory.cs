using System;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Options;
using MyHealth.HealthRecord.Utility;

namespace MyHealth.HealthRecord.Data.Fhir.Base
{
    public class FhirClientFactory : IFhirClientFactory
    {
        private readonly FhirServerSettings _settings;
        private readonly IOperationContext _operationContext;

        public FhirClientFactory(IOptions<FhirServerSettings> settings, IOperationContext operationContext)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _operationContext = operationContext ?? throw new ArgumentNullException(nameof(operationContext));
        }

        public IFhirClient Create()
        {
            var client = new FhirClient(_settings.BaseAddress)
            {
                PreferredFormat = ResourceFormat.Json,
                Timeout = (int)_settings.Timeout.TotalMilliseconds
            };

            client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
            {
                string token = _operationContext.GetAccessTokenAsync()
                    .GetAwaiter()
                    .GetResult();
                e.RawRequest.Headers.Add("Authorization", $"Bearer {token}");
            };

            return client;
        }
    }
}
