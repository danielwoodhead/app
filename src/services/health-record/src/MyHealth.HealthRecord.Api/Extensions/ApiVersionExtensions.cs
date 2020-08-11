using System;
using Microsoft.AspNetCore.Mvc;

namespace MyHealth.HealthRecord.Api.Extensions
{
    public static class ApiVersionExtensions
    {
        public static string ToUrlString(this ApiVersion apiVersion)
        {
            if (apiVersion == null)
                throw new ArgumentNullException(nameof(apiVersion));

            return $"v{apiVersion.ToString()}";
        }
    }
}
