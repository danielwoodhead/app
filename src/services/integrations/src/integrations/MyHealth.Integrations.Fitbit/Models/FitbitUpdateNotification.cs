using System;

namespace MyHealth.Integrations.Fitbit.Models
{
    public class FitbitUpdateNotification
    {
        public string CollectionType { get; set; }
        public DateTime Date { get; set; }
        public string OwnerId { get; set; }
        public string OwnerType { get; set; }
        public string SubscriptionId { get; set; }
    }
}
