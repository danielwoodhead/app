﻿using System;

namespace MyHealth.App.Api.Integrations.Models
{
    public class CreateFitbitIntegrationRequest
    {
        public string Code { get; set; }
        public Uri RedirectUri { get; set; }
    }
}
