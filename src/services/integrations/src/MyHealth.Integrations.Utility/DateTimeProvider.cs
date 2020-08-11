using System;
using MyHealth.Integrations.Core.Utility;

namespace MyHealth.Integrations.Utility
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
