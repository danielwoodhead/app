using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Repository.TableStorage
{
    public static class IntegrationEntityExtensions
    {
        public static Integration Map(this IntegrationEntity entity)
        {
            return new Integration
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Type = entity.Type
            };
        }
    }
}
