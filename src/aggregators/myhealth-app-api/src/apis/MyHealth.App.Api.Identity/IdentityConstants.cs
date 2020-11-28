namespace MyHealth.App.Api.Identity
{
    internal static class IdentityConstants
    {
        public static class DelegationGrant
        {
            public const string Type = "delegation";
            public const string Scopes = "scopes";
            public const string Token = "token";
        }

        public static class PersistedGrant
        {
            public const string RefreshToken = "refresh_token";
        }
    }
}
