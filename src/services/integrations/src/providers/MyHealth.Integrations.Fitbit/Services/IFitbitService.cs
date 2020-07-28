using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Models.Requests;

namespace MyHealth.Integrations.Fitbit.Services
{
    public interface IFitbitService
    {
        Task CreateIntegrationAsync(string userId, AuthorizationCodeRequest request);
        bool Verify(string verificationCode);
        Task ProcessUpdateNotificationAsync(IEnumerable<FitbitUpdateNotification> request);
    }
}
