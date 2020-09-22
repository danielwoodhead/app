using System;

namespace MyHealth.Web.Core.Models
{
    public class CreateFitbitIntegrationRequest
    {
        public string Code { get; set; }
        public Uri RedirectUri { get; set; }
    }
}
