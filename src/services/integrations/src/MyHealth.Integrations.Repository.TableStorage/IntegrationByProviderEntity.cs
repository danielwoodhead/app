﻿using System.Text.Json;
using Microsoft.Azure.Cosmos.Table;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Repository.TableStorage
{
    public class IntegrationByProviderEntity : TableEntity
    {
        public string ProviderData { get; set; }

        public IntegrationByProviderEntity()
        {
        }

        public IntegrationByProviderEntity(string userId, Provider provider, object providerData)
        {
            PartitionKey = userId;
            RowKey = ToRowKey(provider.ToString());
            ProviderData = JsonSerializer.Serialize(providerData);
        }

        public static string ToRowKey(string provider) => $"provider_{provider}";
    }
}
