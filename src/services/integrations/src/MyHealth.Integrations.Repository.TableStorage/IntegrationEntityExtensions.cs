using System;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Repository.TableStorage
{
    public static class IntegrationEntityExtensions
    {
        public static Integration Map(this IntegrationEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new Integration
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Provider = entity.Provider
            };
        }
    }
}
