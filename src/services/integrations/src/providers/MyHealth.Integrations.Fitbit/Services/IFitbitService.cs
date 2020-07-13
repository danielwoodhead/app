using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Integrations.Fitbit.Models;

namespace MyHealth.Integrations.Fitbit.Services
{
    public interface IFitbitService
    {
        Task CreateIntegrationAsync(string code);
        bool Verify(string verificationCode);
        Task ProcessUpdateNotificationAsync(IEnumerable<FitbitUpdateNotification> request);
    }
}
