using System.Diagnostics;

namespace MyHealth.Integrations.Utility
{
    public class OperationContext : IOperationContext
    {
        public string OperationId => Activity.Current?.RootId;
    }
}
