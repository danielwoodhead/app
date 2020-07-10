namespace MyHealth.Integrations.Models
{
    public class Integration
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Provider { get; set; }
        public string Token { get; set; }
    }
}
