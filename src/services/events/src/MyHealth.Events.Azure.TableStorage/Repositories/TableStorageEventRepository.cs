using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using Microsoft.Extensions.Options;
using MyHealth.Events.Azure.TableStorage.Configuration;
using MyHealth.Events.EventIngestion.Repositories;

namespace MyHealth.Events.Azure.TableStorage.Repositories
{
    public class TableStorageEventRepository : IEventRepository
    {
        private const string EventPropertyName = "Event";
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableStorageSettings _tableStorageSettings;

        public TableStorageEventRepository(
            TableServiceClient tableServiceClient,
            IOptions<TableStorageSettings> tableStorageSettings)
        {
            _tableServiceClient = tableServiceClient;
            _tableStorageSettings = tableStorageSettings.Value;
        }

        public async Task StoreAsync(CloudEvent @event)
        {
            await StoreAsync(@event, isRetry: false);
        }

        private async Task StoreAsync(CloudEvent @event, bool isRetry = false)
        {
            try
            {
                byte[] encodedEvent = new JsonEventFormatter().EncodeStructuredModeMessage(@event, out _);
                string eventJson = Encoding.UTF8.GetString(encodedEvent);

                var entity = new TableEntity(partitionKey: @event.Type, rowKey: @event.Id)
                {
                    { EventPropertyName, eventJson }
                };

                TableClient tableClient = _tableServiceClient.GetTableClient(_tableStorageSettings.EventTableName);

                await tableClient.AddEntityAsync(entity);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                if (isRetry)
                {
                    throw;
                }

                await _tableServiceClient.CreateTableIfNotExistsAsync(_tableStorageSettings.EventTableName);
                await StoreAsync(@event, isRetry: true);
            }
        }
    }
}
