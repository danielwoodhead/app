using System.Threading.Tasks;

namespace MyHealth.Extensions.Fhir.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> GetAccessTokenAsync();
    }
}
