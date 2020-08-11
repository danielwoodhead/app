using System.Diagnostics;
using MyHealth.Integrations.Core.Utility;

namespace MyHealth.Integrations.Utility
{
    public class OperationContext : IOperationContext
    {
        public string OperationId => Activity.Current?.RootId;
    }
}
