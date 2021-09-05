using System;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;
using MyHealth.Events.Azure.BlobStorage.Utility;
using MyHealth.Events.EventIngestion.Repositories;

namespace MyHealth.Events.Azure.BlobStorage.Repositories
{
    public class BlobStorageEventRepository : IEventRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ISystemClock _systemClock;

        public BlobStorageEventRepository(BlobServiceClient blobServiceClient, ISystemClock systemClock)
        {
            _blobServiceClient = blobServiceClient;
            _systemClock = systemClock;
        }

        public async Task StoreAsync(CloudEvent @event)
        {
            DateTimeOffset eventDateTime = @event.Time.HasValue ? @event.Time.Value : _systemClock.UtcNow;
            string containerName = eventDateTime.Year.ToString();
            string blobName = GetBlobName(@event, eventDateTime);
            string blobContent = GetBlobContent(@event);

            await UploadBlobAsync(containerName, blobName, blobContent);
        }

        private static string GetBlobName(CloudEvent @event, DateTimeOffset eventDateTime)
        {
            string id = !string.IsNullOrEmpty(@event.Id) ? @event.Id : Guid.NewGuid().ToString();

            return $"{eventDateTime.Month}/{eventDateTime.Day}/{eventDateTime.Hour}/{@event.Type}_{id}.json";
        }

        private static string GetBlobContent(CloudEvent @event)
        {
            byte[] encodedEvent = new JsonEventFormatter().EncodeStructuredModeMessage(@event, out _);

            return Encoding.UTF8.GetString(encodedEvent);
        }

        private async Task UploadBlobAsync(string containerName, string blobName, string blobContent, bool isRetry = false)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            try
            {
                await blobClient.UploadAsync(
                    BinaryData.FromString(blobContent),
                    new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = MediaTypeNames.Application.Json
                        }
                    });
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.ContainerNotFound)
            {
                if (!isRetry)
                {
                    await containerClient.CreateAsync();
                    await UploadBlobAsync(containerName, blobName, blobContent, isRetry: true);
                }
            }
        }
    }
}
