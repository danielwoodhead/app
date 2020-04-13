namespace MyHealth.Mobile.Core
{
    public static class GlobalSettings
    {
        public const string IdentityBase = "https://myhealth-identity-api.azurewebsites.net";
        //public const string IdentityBase = "http://10.0.2.2:62020";

        public const string AuthRedirectUri = "http://localhost:5002/signin-oidc";
        public const string AuthClientId = "mvc";
        public const string AuthClientSecret = "secret";
        public const string AuthAuthorizeEndpoint = IdentityBase + "/connect/authorize";
        public const string AuthTokenEndpoint = IdentityBase + "/connect/token";
        public const string AuthLogoutEndpoint = IdentityBase + "/connect/endsession";
        public const string AuthLogoutCallback = "http://localhost:5002/signout-callback-oidc";

        public const string ObservationsApiUrlBase = "https://myhealth-observations-api.azurewebsites.net/api/v1/observations";
    }
}
