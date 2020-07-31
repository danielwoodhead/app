using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MyHealth.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void Trace(this ILogger logger, string message, IDictionary<string, string> properties)
        {
            logger.LogTrace(GetMessageFormat(message, properties), properties.Values.ToArray());
        }

        public static void Debug(this ILogger logger, string message, IDictionary<string, string> properties)
        {
            logger.LogDebug(GetMessageFormat(message, properties), properties.Values.ToArray());
        }

        public static void Information(this ILogger logger, string message, IDictionary<string, string> properties)
        {
            logger.LogInformation(GetMessageFormat(message, properties), properties.Values.ToArray());
        }

        public static void Information(this ILogger logger, EventId eventId, string message, IDictionary<string, string> properties)
        {
            logger.LogInformation(eventId, GetMessageFormat(message, properties), properties.Values.ToArray());
        }

        public static void Warning(this ILogger logger, string message, IDictionary<string, string> properties)
        {
            logger.LogWarning(GetMessageFormat(message, properties), properties.Values.ToArray());
        }

        public static void Error(this ILogger logger, string message, IDictionary<string, string> properties)
        {
            logger.LogError(GetMessageFormat(message, properties), properties.Values.ToArray());
        }

        public static void Error(this ILogger logger, Exception ex, string message, IDictionary<string, string> properties)
        {
            logger.LogError(ex, GetMessageFormat(message, properties), properties.Values.ToArray());
        }

        public static void Error(this ILogger logger, EventId eventId, Exception ex, string message, IDictionary<string, string> properties)
        {
            logger.LogError(eventId, ex, GetMessageFormat(message, properties), properties.Values.ToArray());
        }

        public static void Critical(this ILogger logger, string message, IDictionary<string, string> properties)
        {
            logger.LogCritical(GetMessageFormat(message, properties), properties.Values.ToArray());
        }

        private static string GetMessageFormat(string message, IDictionary<string, string> properties)
        {
            return $"{message} ({string.Join(", ", properties.Keys.Select(property => $"{property}={{{property}}}"))})";
        }
    }
}
