using System.Threading.Tasks;

namespace MyHealth.Integrations.Utility
{
    public interface IUserOperationContext : IOperationContext
    {
        string UserId { get; }

        Task<string> GetAccessTokenAsync();
    }
}
