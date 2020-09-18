using System.Threading.Tasks;

namespace MyHealth.Extensions.AspNetCore.Context
{
    public interface IOperationContext
    {
        string OperationId { get; }
    }
}
