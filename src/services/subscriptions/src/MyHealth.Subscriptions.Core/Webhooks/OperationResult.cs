namespace MyHealth.Subscriptions.Core.Webhooks
{
    public class OperationResult<T> : OperationResult where T : class
    {
        public T Content { get; }

        private OperationResult(T content, string resultCode)
            : base(resultCode)
        {
            Content = content;
        }

        public static OperationResult<T> Success(T content) =>
            new OperationResult<T>(content, ResultCodes.Success);

        public static OperationResult<T> Failure(string resultCode) =>
            new OperationResult<T>(null, resultCode);
    }

    public class OperationResult
    {
        public string ResultCode { get; }

        protected OperationResult(string resultCode)
        {
            ResultCode = resultCode;
        }

        public static OperationResult Success() =>
            new OperationResult(ResultCodes.Success);
    }

    public static class ResultCodes
    {
        public const string Success = nameof(Success);
        public const string InvalidWebhookUrl = nameof(InvalidWebhookUrl);
        public const string WebhookValidationFailed = nameof(WebhookValidationFailed);
        public const string MaximumWebhooksExceeded = nameof(MaximumWebhooksExceeded);
    }
}
