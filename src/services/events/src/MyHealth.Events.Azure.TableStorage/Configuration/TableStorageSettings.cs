namespace MyHealth.Events.Azure.TableStorage.Configuration
{
    public class TableStorageSettings
    {
        public string ConnectionString { get; set; }
        public string EventTableName { get; set; }
    }
}
