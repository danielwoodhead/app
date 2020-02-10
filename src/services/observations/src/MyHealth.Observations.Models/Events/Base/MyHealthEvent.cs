using System;
using System.Collections.Generic;
using System.Text;

namespace MyHealth.Observations.Models.Events.Base
{
    /// <summary>
    /// Base class for all Patient Platform events with custom properties.
    /// </summary>
    /// <typeparam name="TData">A type derived from EventData that contains custom properties for this event.</typeparam>
    public abstract class MyHealthEvent<TData> : MyHealthEvent, IEvent<TData>
         where TData : EventData
    {
        /// <summary>
        /// Should be called from the constructors of types derived from Event{T}.
        /// </summary>
        /// <param name="id">A unique identifier for the event.</param>
        /// <param name="subject">The subject of the event.</param>
        /// <param name="eventTime">The time the event occurred.</param>
        /// <param name="dataVersion">The data version of the event.</param>
        /// <param name="data">Event data.</param>
        protected MyHealthEvent(string id, string subject, DateTime eventTime, string dataVersion, TData data)
            : base(id, subject, eventTime, dataVersion, data)
        {
        }

        /// <inheritdoc />
        public new TData Data
        {
            get
            {
                return (TData)base.Data;
            }
        }
    }

    /// <summary>
    /// Base class for all Patient Platform events with no custom properties.
    /// </summary>
    public abstract class MyHealthEvent : IEvent
    {
        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public abstract string EventType { get; }

        /// <inheritdoc />
        public string Subject { get; }

        /// <inheritdoc />
        public DateTime EventTime { get; }

        /// <inheritdoc />
        public string DataVersion { get; }

        /// <inheritdoc />
        public EventData Data { get; }

        /// <summary>
        /// Should be called from the constructors of types derived from Event.
        /// </summary>
        /// <param name="id">A unique identifier for the event.</param>
        /// <param name="subject">The subject of the event.</param>
        /// <param name="eventTime">The time the event occurred.</param>
        /// <param name="dataVersion">The data version of the event.</param>
        /// <param name="data">Event data.</param>
        protected MyHealthEvent(string id, string subject, DateTime eventTime, string dataVersion, EventData data)
        {
            Id = id;
            Subject = subject;
            EventTime = eventTime;
            DataVersion = dataVersion;
            Data = data;
        }
    }
}
