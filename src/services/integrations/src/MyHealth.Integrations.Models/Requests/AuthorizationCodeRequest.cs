using System;

namespace MyHealth.Integrations.Models.Requests
{
    public class AuthorizationCodeRequest
    {
        public string Code { get; set; }
        public Uri RedirectUri { get; set; }
    }
}
