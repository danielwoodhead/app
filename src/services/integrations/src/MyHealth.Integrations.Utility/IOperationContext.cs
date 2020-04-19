using System.Threading.Tasks;

namespace MyHealth.Integrations.Utility
{
    public interface IOperationContext
    {
        string OperationId { get; }
        string UserId { get; }

        Task<string> GetAccessTokenAsync();
    }
}
