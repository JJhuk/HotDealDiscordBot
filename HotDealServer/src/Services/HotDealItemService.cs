using HotDealServer.Models;
using MongoDB.Driver;

namespace HotDealServer.Services;

public class HotDealItemService
{
    public readonly IMongoCollection<HotDealProduct> Products;

    public HotDealItemService(IHotDealDatabaseSettings settings)
    {
        Products = new MongoClient(settings.ConnectionString)
            .GetDatabase(settings.DatabaseName)
            .GetCollection<HotDealProduct>(settings.ConnectionString);
    }
}
