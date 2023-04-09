using PuppeteerSharp;

namespace HotDealServer.Crawlers;

public sealed class PpomppuCrawler : ICrawler, IDisposable
{
    private readonly BrowserFetcher _browserFetcher;
    private const string HotDealBoardUrl = "https://www.ppomppu.co.kr/zboard/zboard.php?id=ppomppu";

    public PpomppuCrawler()
    {
        _browserFetcher = new BrowserFetcher();
    }

    public async Task<List<HotDealData>> Crawling()
    {
        // can be exception
        await _browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            ExecutablePath = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe"
        });

        var page = await browser.NewPageAsync();
        await page.GoToAsync(HotDealBoardUrl);

        // todo get table index size
        for (var itemIndex = 7; itemIndex <= 47; itemIndex += 2)
        {
            var itemId = await GetHotDealItemId(page, itemIndex);

            if (itemId == null)
            {
                Console.WriteLine($"item {itemId} is null");
                continue;
            }

            Console.WriteLine($"{itemId}");
        }

        return default;
    }

    private static async Task<int?> GetHotDealItemId(IPage page, int itemIndex)
    {
        var itemIdXPath = await page.WaitForXPathAsync($"//*[@id=\"revolution_main_table\"]/tbody/tr[{itemIndex}]/td[1]");

        var itemId = await itemIdXPath.EvaluateFunctionAsync<int>("x => x.textContent");

        if (itemId == 0)
        {
            return null;
        }

        return itemId;
    }


    public void Dispose()
    {
        _browserFetcher.Dispose();
    }
}
