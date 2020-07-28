using System;

namespace MyHealth.Integrations.Core.Utility
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
