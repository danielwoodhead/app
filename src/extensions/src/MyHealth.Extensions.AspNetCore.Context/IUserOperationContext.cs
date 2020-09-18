using System.Threading.Tasks;

namespace MyHealth.Extensions.AspNetCore.Context
{
    public interface IUserOperationContext : IOperationContext
    {
        string UserId { get; }

        Task<string> GetAccessTokenAsync();
    }
}
