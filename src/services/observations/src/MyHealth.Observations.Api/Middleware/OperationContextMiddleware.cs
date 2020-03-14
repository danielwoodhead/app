using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MyHealth.Observations.Api.Middleware
{
    public class OperationContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<OperationContextMiddleware> _logger;

        public OperationContextMiddleware(RequestDelegate next, ILogger<OperationContextMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(context?.User?.FindFirst("sub")?.Value))
            {
                _logger.LogWarning("Rejecting request with no 'sub' claim");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
