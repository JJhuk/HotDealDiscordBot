using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotDealServer.Models;
using HotDealServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace HotDealServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrawlingController : ControllerBase
    {
        private readonly ILogger<CrawlingController> _logger;
        private readonly HotDealItemService _itemService;
        
        public CrawlingController(ILogger<CrawlingController> logger, HotDealItemService itemService)
        {
            _logger = logger;
            _itemService = itemService;
        }

        public async Task<ActionResult> CrawlingProducts(CancellationToken token = default)
        {
            try
            {
                var hotDealProducts = await Crawling();
                
                await _itemService.Products.InsertManyAsync(hotDealProducts, cancellationToken: token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Raised Exception in {nameof(CrawlingController)}");
                throw;
            }

            return Ok();
        }

        private static async Task<List<HotDealProduct>> Crawling()
        {
            var revisionInfo = await new BrowserFetcher()
                .DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
    
            Console.WriteLine(revisionInfo);
            var launchOptions = new LaunchOptions
            {
                Headless = false
            };
            await using var browser = await Puppeteer.LaunchAsync(launchOptions);
            await using var page = await browser.NewPageAsync();
            await page.GoToAsync("https://www.ppomppu.co.kr/zboard/zboard.php?id=ppomppu");
            const string? jsSelectAllAnchors = @"Array.from(document.querySelector('#revolution_main_table > tbody').rows).filter(a => a.className.startsWith('list')).filter(a => !a.className.includes('notice')).map(x => x.children[2].outerText)";
/*#revolution_main_table > tbody > tr:nth-child(7) > td:nth-child(3) > table > tbody > tr > td:nth-child(2)
#revolution_main_table > tbody > tr:nth-child(9) > td:nth-child(3) > table > tbody > tr > td:nth-child(2)*/

            var items = await page.EvaluateExpressionAsync<string[]>(jsSelectAllAnchors);
            foreach (var item in items)
            {
                Console.WriteLine($"item: {item}");
            }
            return default;
        }
    }
}