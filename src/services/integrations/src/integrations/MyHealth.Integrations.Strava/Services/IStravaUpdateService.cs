using System.Threading.Tasks;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public interface IStravaUpdateService
    {
        Task ProcessUpdateNotification(StravaUpdateNotification updateNotification);
    }
}