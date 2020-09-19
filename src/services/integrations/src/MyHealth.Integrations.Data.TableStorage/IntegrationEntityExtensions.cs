using System;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Data.TableStorage
{
    public static class IntegrationEntityExtensions
    {
        public static Integration Map(this IntegrationByIdEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new Integration
            {
                Id = entity.IntegrationId,
                Provider = entity.ProviderEnum,
                Enabled = true
            };
        }

        public static Integration Map(this IntegrationByProviderEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new Integration
            {
                Provider = entity.Provider,
                Data = entity.ProviderData,
                Enabled = true
            };
        }
    }
}
