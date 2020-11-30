using System;
using MyHealth.Integrations.Core.Utility;

namespace MyHealth.Integrations.Tests.Mocks
{
    public class MockDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => new DateTime(2000, 1, 1, 10, 0, 0);
    }
}
