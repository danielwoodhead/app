using System;

namespace MyHealth.Events.Azure.BlobStorage.Utility
{
    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
