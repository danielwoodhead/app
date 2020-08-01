using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Requests;

namespace MyHealth.Integrations.Fitbit.Services
{
    public interface IFitbitService
    {
        Task<Integration> CreateIntegrationAsync(string userId, AuthorizationCodeRequest request);
        Task ProcessUpdateNotificationAsync(IEnumerable<FitbitUpdateNotification> request);
        bool VerifySubscriptionEndpoint(string verificationCode);
        bool VerifyUpdateNotification(string request, string verificationSignature);
    }
}
