using System;

namespace MyHealth.Web.Core.Models
{
    public class AuthorizationCodeRequest
    {
        public string Code { get; set; }
        public Uri RedirectUri { get; set; }
    }
}
