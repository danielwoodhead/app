using MyHealth.Subscriptions.Core.Webhooks;

namespace MyHealth.Subscriptions.Tests.Mocks
{
    public class MockRandomStringGenerator : IRandomStringGenerator
    {
        public string Returns { get; set; }
        public string Create() => Returns;
    }
}
