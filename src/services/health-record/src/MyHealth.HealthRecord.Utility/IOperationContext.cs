using System.Threading.Tasks;

namespace MyHealth.HealthRecord.Utility
{
    public interface IOperationContext
    {
        string OperationId { get; }
        string UserId { get; }

        Task<string> GetAccessTokenAsync();
    }
}
