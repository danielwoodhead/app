using System.Diagnostics;

namespace MyHealth.Extensions.AspNetCore.Context
{
    public class OperationContext : IOperationContext
    {
        public string OperationId => Activity.Current?.RootId;
    }
}
