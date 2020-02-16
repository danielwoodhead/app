using System;
using System.Collections.Generic;
using System.Linq;

namespace MyHealth.Observations.Models.Events.Base
{
    /// <summary>
    /// Base class for all domain events with custom properties.
    /// </summary>
    /// <typeparam name="TData">A type derived from EventData that contains custom properties for this event.</typeparam>
    public abstract class DomainEvent<TData> : DomainEvent, IEvent<TData>
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
        protected DomainEvent(string id, string subject, DateTime eventTime, string dataVersion, TData data)
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
    /// Base class for all domain events with no custom properties.
    /// </summary>
    public abstract class DomainEvent : IEvent
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

        /// <inheritdoc />
        public virtual IDictionary<string, string> Properties => new Dictionary<string, string>
        {
            { nameof(Id), Id },
            { nameof(EventType), EventType },
            { nameof(Subject), Subject },
#pragma warning disable CA1305 // Specify IFormatProvider
            { nameof(EventTime), EventTime.ToString("s") },
#pragma warning restore CA1305 // Specify IFormatProvider
            { nameof(DataVersion), DataVersion },
        }.Union(Data.Properties).ToDictionary(x => x.Key, x => x.Value);

        /// <summary>
        /// Should be called from the constructors of types derived from Event.
        /// </summary>
        /// <param name="id">A unique identifier for the event.</param>
        /// <param name="subject">The subject of the event.</param>
        /// <param name="eventTime">The time the event occurred.</param>
        /// <param name="dataVersion">The data version of the event.</param>
        /// <param name="data">Event data.</param>
        protected DomainEvent(string id, string subject, DateTime eventTime, string dataVersion, EventData data)
        {
            Id = id;
            Subject = subject;
            EventTime = eventTime;
            DataVersion = dataVersion;
            Data = data;
        }
    }
}
