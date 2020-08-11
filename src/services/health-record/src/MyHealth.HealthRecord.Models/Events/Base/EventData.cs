using System.Collections.Generic;

namespace MyHealth.HealthRecord.Models.Events.Base
{
    /// <summary>
    /// The minimum required event data.
    /// </summary>
    public class EventData
    {
        /// <summary>
        /// An identifier for correlating distributed operations.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// An identifier for the system that published the event.
        /// </summary>
        public string SourceSystem { get; set; }

        /// <summary>
        /// An identifier for the system that owns the event subject.
        /// </summary>
        public string SubjectSystem { get; set; }

        /// <summary>
        /// Dictionary representation of the event data.
        /// </summary>
        public virtual IDictionary<string, string> Properties => new Dictionary<string, string>
        {
            { nameof(OperationId), OperationId },
            { nameof(SourceSystem), SourceSystem },
            { nameof(SubjectSystem), SubjectSystem }
        };
    }
}
