namespace HotDealServer.Models;

public class HotDealDatabaseSettings : IHotDealDatabaseSettings
{
    public string ItemCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}

public interface IHotDealDatabaseSettings
{
    string ItemCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}