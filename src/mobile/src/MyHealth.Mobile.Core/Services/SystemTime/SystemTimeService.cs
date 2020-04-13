using System;

namespace MyHealth.Mobile.Core.Services.SystemTime
{
    public class SystemTimeService : ISystemTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
