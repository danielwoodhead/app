using System.Diagnostics;

namespace MyHealth.Observations.Utility
{
    public class OperationContext : IOperationContext
    {
        public string OperationId => Activity.Current?.RootId;
    }
}
