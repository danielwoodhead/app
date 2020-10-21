using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Models;
using Newtonsoft.Json;

namespace MyHealth.Integrations.Data.Cosmos
{
    public class IntegrationEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string UserId { get; set; }
        public Provider Provider { get; set; }
        public string ProviderUserId { get; set; }
        public IProviderData ProviderData { get; set; }
    }
}
