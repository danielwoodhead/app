using System;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Repository.TableStorage
{
    public static class IntegrationEntityExtensions
    {
        public static Integration Map(this IntegrationByIdEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new Integration
            {
                Id = entity.RowKey,
                UserId = entity.PartitionKey,
                Provider = entity.ProviderEnum
            };
        }
    }
}
