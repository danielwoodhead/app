using System;

namespace MyHealth.Web.App.Areas.Integrations.Models
{
    public class CreateFitbitIntegrationRequest
    {
        public string Code { get; set; }
        public Uri RedirectUri { get; set; }
    }
}
