﻿namespace MyHealth.Integrations.Models.Requests
{
    public class CreateIntegrationRequest
    {
        public Provider Provider { get; set; }
        public object Data { get; set; }
    }
}
