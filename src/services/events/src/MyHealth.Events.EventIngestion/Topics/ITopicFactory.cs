namespace MyHealth.Events.EventIngestion.Topics
{
    public interface ITopicFactory
    {
        ITopic Create(string name);
    }
}
