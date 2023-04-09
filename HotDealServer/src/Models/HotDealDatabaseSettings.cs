namespace HotDealServer.Models;

public class HotDealDatabaseSettings : IHotDealDatabaseSettings
{
    public required string ItemCollectionName { get; set; }
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
}

public interface IHotDealDatabaseSettings
{
    string ItemCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}
