using System.Threading.Tasks;

namespace MyHealth.Extensions.Fhir.Authentication
{
    internal interface IAuthenticationService
    {
        Task<string> GetAccessTokenAsync();
    }
}
