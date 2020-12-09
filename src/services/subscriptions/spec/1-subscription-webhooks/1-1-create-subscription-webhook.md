# 1.1 Create subscription webhook

```
POST subscriptions/webhooks
{
  "webhookUrl": "https://mywebhook.com"
}
```

When a new subscription webhook is created a HTTP GET request is sent to it with a query parameter called `verify`. The webhook should echo this back in the response with a 200 OK response code. If this validation succeeds the webhook is stored.

## 1.1.1

> Status: Automated ([CreateSubscriptionWebhook_InvalidWebhookUrl_ReturnsBadRequest](../../tests/MyHealth.Subscriptions.Tests/Api/SubscriptionWebhookTests.cs))

Title: Invalid webhook URL

Description: The value provided for `webhookUrl` is not a valid URL

Expected outcome: 400 BadRequest

Example request:
```
POST subscriptions/webhooks
{
  "webhookUrl": "invalidUrl"
}
```

Example response:
```
400 BadRequest
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
	"title": "InvalidUrl",
	"status": 400,
	"traceId": "|a9ed5e4b-46639940a9e63797."
}
```

## 1.1.2

> Status: Automated ([CreateSubscriptionWebhook_WebhookValidationFails_IncorrectStatusCode_ReturnsBadRequest](../../tests/MyHealth.Subscriptions.Tests/Api/SubscriptionWebhookTests.cs))

Title: Webhook validation failed (incorrect status code)

Description: The webhook does not respond to the validation request with a 200 OK status code

Expected outcome: 400 BadRequest

Example webhook validation request:
```
GET https://mywebhook.com?verify=12345
```

Example webhook response:
```
404 NotFound
```

Example response:
```
400 BadRequest
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
	"title": "WebhookValidationFailed",
	"status": 400,
	"traceId": "|a9ed5e4b-46639940a9e63797."
}
```

## 1.1.3

> Status: Automated ([CreateSubscriptionWebhook_WebhookValidationFails_IncorrectResponseFormat_ReturnsBadRequest](../../tests/MyHealth.Subscriptions.Tests/Api/SubscriptionWebhookTests.cs))

Title: Webhook validation failed (incorrect response format)

Description: The webhook does not respond to the validation request with the expected response format

Expected outcome: 400 BadRequest

Example webhook validation request:
```
GET https://mywebhook.com?verify=12345
```

Example webhook response:
```
200 OK
{
	"foo": "bar"
}
```

Example response:
```
400 BadRequest
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
	"title": "WebhookValidationFailed",
	"status": 400,
	"traceId": "|a9ed5e4b-46639940a9e63797."
}
```

## 1.1.4

> Status: Automated ([CreateSubscriptionWebhook_WebhookValidationFails_IncorrectVerificationCode_ReturnsBadRequest](../../tests/MyHealth.Subscriptions.Tests/Api/SubscriptionWebhookTests.cs))

Title: Webhook validation failed (incorrect verification code)

Description: The webhook does not respond to the validation request with the correct verification code

Expected outcome: 400 BadRequest

Example webhook validation request:
```
GET https://mywebhook.com?verify=12345
```

Example webhook response:
```
200 OK
{
	"verify": "incorrectCode"
}
```

Example response:
```
400 BadRequest
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
	"title": "WebhookValidationFailed",
	"status": 400,
	"traceId": "|a9ed5e4b-46639940a9e63797."
}
```

## 1.1.5

> Status: Manual

Title: Webhook validation timeout

Description: The webhook does not respond within 10 seconds

Expected outcome: 400 BadRequest

Example response:
```
400 BadRequest
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
	"title": "WebhookValidationTimeout",
	"status": 400,
	"traceId": "|a9ed5e4b-46639940a9e63797."
}
```

## 1.1.6

> Status: Automated ([CreateSubscriptionWebhook_ApplicationAlreadyHasWebhook_ReturnsConflict](../../tests/MyHealth.Subscriptions.Tests/Api/SubscriptionWebhookTests.cs))

Title: Webhook already exists

Description: The requesting application already has a subscription webhook

Expected outcome: 409 Conflict

Example response:
```
409 Conflict
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
	"title": "MaximumWebhooksExceeded",
	"status": 409,
	"traceId": "|a9ed5e4b-46639940a9e63797."
}
```

## 1.1.7

> Status: Draft

Title: Webhook storage timeout

Description: The call to store the webhook times out after 10 seconds

## 1.1.8

> Status: Draft

Title: Webhook storage fails, transient error, retry successful

Description: The first attempt to store the subscription webhook fails due to a transient error. The application automatically retries and is successful.

Expected outcome: 201 Created

## 1.1.9

> Status: Draft

Title: Webhook storage fails, transient error, retries unsuccessful

Description: The first attempt to store the subscription webhook fails due to a transient error. The application automatically retries and exhausts all retry attempts.

Expected outcome: 500 InternalServerError

## 1.1.10

> Status: Draft

Title: Webhook storage fails (non-transient error)

Description:

Expected outcome:

## 1.1.11

> Status: Automated ([CreateSubscriptionWebhook_Success_ReturnsCreated](../../tests/MyHealth.Subscriptions.Tests/Api/SubscriptionWebhookTests.cs))

Title: Successful subscription webhook creation

Description: Subscription webhook validation passes and webhook is successfully stored

Expected outcome: 201 Created

Example response:
```
201 Created
{
	"id": "944d8d50-305f-492c-8d3a-6a1cb945c7fe",
	"webhookUrl": "https://mywebhook.com",
	"clientId": "clientId"
}
```

## 1.1.12

> Status: Draft

Title: Performance...

Description:

## 1.1.13

> Status: Draft

Title: Authentication...