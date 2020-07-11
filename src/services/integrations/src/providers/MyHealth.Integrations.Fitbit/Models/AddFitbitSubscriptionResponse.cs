namespace MyHealth.Integrations.Fitbit.Models
{
    public class AddFitbitSubscriptionResponse
    {
        public string CollectionType { get; set; }
        public string OwnerId { get; set; }
        public string OwnerType { get; set; }
        public string SubscriberId { get; set; }
        public string SubscriptionId { get; set; }
    }
}
