﻿namespace MyHealth.Integrations.Models
{
    public class Integration
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Provider Provider { get; set; }
        public bool Enabled { get; set; }
        public IProviderData Data { get; set; }
    }

    public interface IProviderData { }
}
