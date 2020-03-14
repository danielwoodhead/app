using System;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Options;
using MyHealth.Observations.Utility;

namespace MyHealth.Observations.Integration.Fhir.Base
{
    public class FhirClientFactory : IFhirClientFactory
    {
        private readonly FhirServerSettings _settings;
        private readonly IOperationContext _operationContext;
        private readonly AsyncLazy<IFhirClient> _client;

        public FhirClientFactory(IOptions<FhirServerSettings> settings, IOperationContext operationContext)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _operationContext = operationContext ?? throw new ArgumentNullException(nameof(operationContext));
            _client = new AsyncLazy<IFhirClient>(async () => await CreateInternalAsync());
        }

        public async Task<IFhirClient> InstanceAsync() => await _client.Value;

        private async Task<IFhirClient> CreateInternalAsync()
        {
            var client = new FhirClient(_settings.BaseUrl)
            {
                PreferredFormat = ResourceFormat.Json,
                Timeout = (int)_settings.Timeout.TotalMilliseconds
            };

            string token = await _operationContext.GetAccessTokenAsync();
            client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
            {
                e.RawRequest.Headers.Add("Authorization", $"Bearer {token}");
            };

            return client;
        }
    }
}
