namespace MyHealth.Observations.Models.Events.Base
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
    }
}
