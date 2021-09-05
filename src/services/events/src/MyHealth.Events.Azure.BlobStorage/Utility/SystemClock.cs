using System;

namespace MyHealth.Events.Azure.BlobStorage.Utility
{
    public class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
