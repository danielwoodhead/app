using System;
using System.Collections.Generic;

namespace MyHealth.HealthRecord.Models.Events.Base
{
    /// <summary>
    /// An event.
    /// </summary>
    /// <typeparam name="TData">The event data type.</typeparam>
    public interface IEvent<TData> : IEvent
        where TData : EventData
    {
        /// <summary>
        /// The event data.
        /// </summary>
        new TData Data { get; }
    }

    /// <summary>
    /// An event.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// A unique identifier for the event.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The event type.
        /// </summary>
        string EventType { get; }

        /// <summary>
        /// The subject of the event.
        /// </summary>
        string Subject { get; }

        /// <summary>
        /// The time the event occurred.
        /// </summary>
        DateTime EventTime { get; }

        /// <summary>
        /// The data version of the event.
        /// </summary>
        string DataVersion { get; }

        /// <summary>
        /// The event data.
        /// </summary>
        EventData Data { get; }

        /// <summary>
        /// A dictionary representation of the event.
        /// </summary>
        IDictionary<string, string> Properties { get; }
    }
}
