using System;

namespace MyHealth.Extensions.Events.Azure.EventGrid
{
    public class EventGridSettings
    {
        public bool Enabled { get; set; }
        public string TopicEndpoint { get; set; }
        public string TopicKey { get; set; }
        public string TopicHostname => Enabled ? new Uri(TopicEndpoint).Host : string.Empty;
    }
}
