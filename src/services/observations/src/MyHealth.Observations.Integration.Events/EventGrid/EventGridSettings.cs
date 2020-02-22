using System;

namespace MyHealth.Observations.Integration.Events.EventGrid
{
    public class EventGridSettings
    {
        public bool Enabled { get; set; }
        public string TopicEndpoint { get; set; }
        public string TopicKey { get; set; }
        public string TopicHostname => new Uri(TopicEndpoint).Host;
    }
}
