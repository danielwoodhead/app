using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace MyHealth.Subscriptions.Tests.Mocks
{
    public class MockHttpClient
    {
        private readonly Mock<HttpMessageHandler> _mockHttp = new Mock<HttpMessageHandler>();

        public HttpClient Create() => new HttpClient(_mockHttp.Object);

        public void SetupResponse(HttpStatusCode statusCode)
        {
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = statusCode
            };

            SetupResponse(responseMessage);
        }

        public void SetupResponse<T>(HttpStatusCode statusCode, T content = null) where T : class
        {
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = JsonContent.Create(content)
            };

            SetupResponse(responseMessage);
        }

        private void SetupResponse(HttpResponseMessage responseMessage)
        {
            _mockHttp
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage)
                .Verifiable();
        }
    }
}
