using System;
using Microsoft.Azure.Cosmos.Table;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Repository.TableStorage
{
    public class IntegrationByIdEntity : TableEntity
    {
        public string Provider { get; set; }
        public string IntegrationId => RowKey.Split('_')[1];
        public Provider ProviderEnum => Enum.Parse<Provider>(Provider);

        public IntegrationByIdEntity()
        {
        }

        public IntegrationByIdEntity(string userId, string integrationId, Provider provider)
        {
            PartitionKey = userId;
            RowKey = ToRowKey(integrationId);
            Provider = provider.ToString();
        }

        public static string ToRowKey(string integrationId) => $"integrationId_{integrationId}";
    }
}
