using System;
using System.Collections.Generic;
using System.Linq;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Core.Extensions
{
    public static class IntegrationSystemServiceExtensions
    {
        public static IIntegrationSystemService GetService(this IEnumerable<IIntegrationSystemService> services, Provider provider)
        {
            IIntegrationSystemService service = services.FirstOrDefault(s => s.Provider == provider);

            if (service == null)
                throw new ArgumentException($"{nameof(IIntegrationSystemService)} implementation not found for '{provider}'.");

            return service;
        }
    }
}
