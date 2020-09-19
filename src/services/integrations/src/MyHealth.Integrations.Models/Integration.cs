namespace MyHealth.Integrations.Models
{
    public class Integration
    {
        public string Id { get; set; }
        public Provider Provider { get; set; }
        public bool Enabled { get; set; }
        public string Data { get; set; }
    }
}
