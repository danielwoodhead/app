using System.Threading.Tasks;

namespace MyHealth.Observations.Utility
{
    public interface IOperationContext
    {
        string OperationId { get; }
        string UserId { get; }

        Task<string> GetAccessTokenAsync();
    }
}
