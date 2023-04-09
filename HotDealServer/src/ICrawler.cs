namespace HotDealServer;

public interface ICrawler
{
    public Task<List<HotDealData>> Crawling();
}
