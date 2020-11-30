using System.Text.Json;
using MyHealth.Integrations.Core.IoMT.Models;
using Xunit;

namespace MyHealth.Integrations.Core.Tests.IoMT.Models
{
    public class IoMTModelTests
    {
        [Fact]
        public void Base_Model_Is_Deserialized_Correctly()
        {
            IoMTModel model = new BodyWeight
            {
                Weight = 90.2
            };

            var serialized = JsonSerializer.Serialize(model);

            Assert.Contains("\"weight\":90.2", serialized);
        }
    }
}
