using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace MyHealth.Web.Core.Integrations
{
    public interface IFitbitClient
    {
        string GetAuthenticationUri(string baseRedirectUri);
    }

    public class FitbitClient : IFitbitClient
    {
        private readonly IConfiguration _configuration;

        public FitbitClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAuthenticationUri(string baseRedirectUri)
        {
            string baseUri = "https://www.fitbit.com/oauth2/authorize";
            string clientId = _configuration["Integrations:Fitbit:ClientId"];

            string result = QueryHelpers.AddQueryString(baseUri, new Dictionary<string, string>
            {
                { "response_type", "code" },
                { "client_id", clientId },
                { "redirect_uri", $"{baseRedirectUri}integrations/fitbitcallback" },
                { "scope", "activity nutrition heartrate location nutrition profile settings sleep social weight" }
            });

            return result;
        }
    }
}
