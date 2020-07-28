using System;

namespace MyHealth.Integrations.Core.Utility
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
