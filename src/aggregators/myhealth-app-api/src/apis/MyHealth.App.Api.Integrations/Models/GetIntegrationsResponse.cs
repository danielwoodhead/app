﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace MyHealth.App.Api.Integrations.Models
{
    public class GetIntegrationsResponse
    {
        [JsonPropertyName("integrations")]
        public IEnumerable<Integration> Integrations { get; set; }
    }
}
