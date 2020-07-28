namespace MyHealth.Integrations.Models
{
    public class Integration
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Provider Provider { get; set; }
        public object Data { get; set; }
    }
}
