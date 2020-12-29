namespace MyHealth.Web.App.Areas.Integrations.Models
{
    public class Integration
    {
        public string Id { get; set; }
        public Provider Provider { get; set; }
        public bool Enabled { get; set; }
    }
}
