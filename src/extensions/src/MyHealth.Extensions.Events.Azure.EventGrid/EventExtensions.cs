using System;
using Microsoft.Azure.EventGrid.Models;

namespace MyHealth.Extensions.Events.Azure.EventGrid
{
    /// <summary>
    /// Event extension methods.
    /// </summary>
    public static class EventExtensions
    {
        /// <summary>
        /// Converts the event to an instance of EventGridEvent.
        /// </summary>
        /// <param name="e">The event.</param>
        /// <returns>The converted EventGridEvent.</returns>
        public static EventGridEvent ToEventGridEvent(this IEvent e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            return new EventGridEvent(
                id: e.Id,
                subject: e.Subject,
                data: e.Data,
                eventType: e.EventType,
                eventTime: e.EventTime,
                dataVersion: e.DataVersion);
        }
    }
}
