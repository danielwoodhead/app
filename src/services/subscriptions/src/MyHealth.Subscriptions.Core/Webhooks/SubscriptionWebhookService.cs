using System;
using System.Threading.Tasks;
using MyHealth.Subscriptions.Models;
using MyHealth.Subscriptions.Models.Requests;

namespace MyHealth.Subscriptions.Core.Webhooks
{
    public class SubscriptionWebhookService : ISubscriptionWebhookService
    {
        private readonly ISubscriptionWebhookRepository _subscriptionWebhookRepository;
        private readonly ISubscriptionWebhookClient _subscriptionWebhookClient;
        private readonly IRandomStringGenerator _randomStringGenerator;
        private readonly IOperationContext _operationContext;

        public SubscriptionWebhookService(
            ISubscriptionWebhookRepository subscriptionWebhookRepository,
            ISubscriptionWebhookClient subscriptionWebhookValidator,
            IRandomStringGenerator randomStringGenerator,
            IOperationContext operationContext)
        {
            _subscriptionWebhookRepository = subscriptionWebhookRepository;
            _subscriptionWebhookClient = subscriptionWebhookValidator;
            _randomStringGenerator = randomStringGenerator;
            _operationContext = operationContext;
        }

        public async Task<OperationResult<SubscriptionWebhook>> AddSubscriptionWebhookAsync(CreateSubscriptionWebhookRequestModel request)
        {
            if (!IsValidUrl(request.WebhookUrl))
            {
                return OperationResult<SubscriptionWebhook>.Failure(ResultCodes.InvalidWebhookUrl);
            }

            SubscriptionWebhook existingWebhook = await _subscriptionWebhookRepository.GetSubscriptionWebhookAsync(_operationContext.ClientId);

            if (existingWebhook != null)
            {
                return OperationResult<SubscriptionWebhook>.Failure(ResultCodes.MaximumWebhooksExceeded);
            }

            string verificationCode = _randomStringGenerator.Create();
            SubscriptionWebhookValidationResponse validationResponse = await _subscriptionWebhookClient.ValidateAsync(request.WebhookUrl, verificationCode);

            if (validationResponse?.Verify != verificationCode)
            {
                return OperationResult<SubscriptionWebhook>.Failure(ResultCodes.WebhookValidationFailed);
            }

            SubscriptionWebhook webhook = await _subscriptionWebhookRepository.AddSubscriptionWebhookAsync(
                webhookUrl: request.WebhookUrl,
                clientId: _operationContext.ClientId);

            return OperationResult<SubscriptionWebhook>.Success(webhook);
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }

    public interface IRandomStringGenerator
    {
        string Create();
    }

    public class GuidRandomStringGenerator : IRandomStringGenerator
    {
        public string Create() => Guid.NewGuid().ToString();
    }

    public interface IOperationContext
    {
        string ClientId { get; }
    }

    public class OperationContext : IOperationContext
    {
        public string ClientId => "TODO";
    }
}
