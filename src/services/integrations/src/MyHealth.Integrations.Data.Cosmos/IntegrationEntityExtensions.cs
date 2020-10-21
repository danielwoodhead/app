using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Data.Cosmos
{
    public static class IntegrationEntityExtensions
    {
        public static Integration Map(this IntegrationEntity entity)
        {
            return new Integration
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Provider = entity.Provider,
                Enabled = true,
                Data = entity.ProviderData
            };
        }
    }
}
