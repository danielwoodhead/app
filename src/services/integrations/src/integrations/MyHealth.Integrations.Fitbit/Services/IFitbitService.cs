using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Integrations.Fitbit.Models;

namespace MyHealth.Integrations.Fitbit.Services
{
    public interface IFitbitService
    {
        string GetAuthenticationUri(string redirectUri);
        Task ProcessUpdateNotificationAsync(IEnumerable<FitbitUpdateNotification> request);
        bool VerifySubscriptionEndpoint(string verificationCode);
        bool VerifyUpdateNotification(string request, string verificationSignature);
    }
}
