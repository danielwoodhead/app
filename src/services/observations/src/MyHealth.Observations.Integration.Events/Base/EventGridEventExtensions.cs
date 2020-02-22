using System;
using Microsoft.Azure.EventGrid.Models;
using MyHealth.Observations.Models.Events.Base;
using Newtonsoft.Json.Linq;

namespace MyHealth.Observations.Integration.Events.Base
{
    /// <summary>
    /// Extension methods for EventGridEvent.
    /// </summary>
    public static class EventGridEventExtensions
    {
        /// <summary>
        /// Converts the EventGridEvent into TEvent. For conversion to succeed e.EventType must match TEvent.EventType and e.Data must be an instance of EventData.
        /// </summary>
        /// <typeparam name="TEvent">The type to convert into.</typeparam>
        /// <param name="e">The event to convert.</param>
        /// <returns>The converted TEvent or default(TEvent) if conversion failed.</returns>
        public static TEvent ReadAs<TEvent>(this EventGridEvent e)
             where TEvent : IEvent
        {
            return e.ReadAs<TEvent, EventData>();
        }

        /// <summary>
        /// Converts the EventGridEvent into TEvent. For conversion to succeed e.EventType must match TEvent.EventType and e.Data must be an instance of TData.
        /// </summary>
        /// <typeparam name="TEvent">The type to convert into.</typeparam>
        /// <typeparam name="TData">The event's custom data type.</typeparam>
        /// <param name="e">The event to convert.</param>
        /// <returns>The converted TEvent or default(TEvent) if conversion failed.</returns>
        public static TEvent ReadAs<TEvent, TData>(this EventGridEvent e)
            where TEvent : IEvent
            where TData : EventData
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            TData eventData = (e.Data as JObject)?.ToObject<TData>();
            if (eventData == null)
                return default;

            TEvent instance = (TEvent)Activator.CreateInstance(typeof(TEvent), e.Id, e.Subject, e.EventTime, e.DataVersion, eventData);

            if (e.EventType != instance.EventType)
                return default;

            return instance;
        }
    }
}
