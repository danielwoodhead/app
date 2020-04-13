using System;

namespace MyHealth.Mobile.Core.Services.SystemTime
{
    public interface ISystemTimeService
    {
        DateTime UtcNow { get; }
    }
}
