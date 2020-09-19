namespace MyHealth.Web.Core.Models
{
    public class Integration
    {
        public string Id { get; set; }
        public Provider Provider { get; set; }
        public bool Enabled { get; set; }
    }
}
