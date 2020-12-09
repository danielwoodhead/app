using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Subscriptions.Core.Webhooks;
using MyHealth.Subscriptions.Models;
using MyHealth.Subscriptions.Models.Requests;

namespace MyHealth.Subscriptions.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("v{version:apiVersion}/subscriptions")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionWebhookService _subscriptionWebhookService;

        public SubscriptionsController(ISubscriptionWebhookService subscriptionWebhookService)
        {
            _subscriptionWebhookService = subscriptionWebhookService;
        }

        [HttpPost("webhooks")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<SubscriptionWebhook>> CreateSubscriptionWebhook([FromBody] CreateSubscriptionWebhookRequestModel request, ApiVersion apiVersion)
        {
            OperationResult<SubscriptionWebhook> result = await _subscriptionWebhookService.AddSubscriptionWebhookAsync(request);

            return ToActionResult(result, apiVersion);
        }

        [HttpGet("{id}", Name = nameof(GetSubscriptionWebhook))]
        public async Task<ActionResult<SubscriptionWebhook>> GetSubscriptionWebhook([FromRoute] string id)
        {
            // TODO
            await Task.Delay(1);
            return NotFound();
        }

        private ActionResult<SubscriptionWebhook> ToActionResult(OperationResult<SubscriptionWebhook> operationResult, ApiVersion apiVersion)
        {
            return operationResult.ResultCode switch
            {
                ResultCodes.Success => CreatedAtAction(nameof(GetSubscriptionWebhook), new { id = operationResult.Content.Id, version = apiVersion.ToUrlString() }, operationResult.Content),
                ResultCodes.InvalidWebhookUrl => BadRequest(ResultCodes.InvalidWebhookUrl),
                ResultCodes.WebhookValidationFailed => BadRequest(ResultCodes.WebhookValidationFailed),
                ResultCodes.MaximumWebhooksExceeded => Conflict(ResultCodes.MaximumWebhooksExceeded),
                _ => throw new ArgumentException($"Unsupported result code '{operationResult.ResultCode}'"),
            };
        }

        private ActionResult<SubscriptionWebhook> BadRequest(string resultCode) =>
            Problem(statusCode: StatusCodes.Status400BadRequest, title: resultCode);

        private ActionResult<SubscriptionWebhook> Conflict(string resultCode) =>
            Problem(statusCode: StatusCodes.Status409Conflict, title: resultCode);
    }
}
