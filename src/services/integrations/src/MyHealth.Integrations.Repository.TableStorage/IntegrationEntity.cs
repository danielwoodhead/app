using Microsoft.Azure.Cosmos.Table;

namespace MyHealth.Integrations.Repository.TableStorage
{
    public class IntegrationEntity : TableEntity
    {
        public string Id => RowKey;
        public string UserId => PartitionKey;
        public string Provider { get; set; }
        public string Token { get; set; }

        public IntegrationEntity(
            string id,
            string userId)
        {
            PartitionKey = userId;
            RowKey = id;
        }

        public IntegrationEntity()
        {
        }
    }
}
