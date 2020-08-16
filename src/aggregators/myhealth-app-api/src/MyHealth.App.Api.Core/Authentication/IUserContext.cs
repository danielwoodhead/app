using System.Threading.Tasks;

namespace MyHealth.App.Api.Core.Authentication
{
    public interface IUserContext
    {
        string UserId { get; }
        Task<string> GetAccessTokenAsync();
    }
}
