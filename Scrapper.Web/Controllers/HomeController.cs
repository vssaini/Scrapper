using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Scrapes;
using Scrapper.Web.Contracts;

namespace Scrapper.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var scrapeResult = new ScrapeResult(new PageResult<Scrape>(), null, null);

            try
            {
                scrapeResult = await _homeService.GetScrapesAsync(null);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving scrapes.");
            }

            return View(scrapeResult);
        }

        [HttpPost]
        public async Task<IActionResult> SearchScrapes([FromBody] SearchRequest request)
        {
            var scrapeResult = new ScrapeResult(new PageResult<Scrape>(), request.Page, request.Sort);

            _logger.LogInformation("Searching scrapes for search text '{SearchText}'. Start date - {StartDate} & End date - {EndDate}. Page number - {PageNumber} & Page size - {PageSize}.", request.SearchText, request.DateRange.Start, request.DateRange.End, request.Page.Number, request.Page.Size);

            try
            {
                scrapeResult = await _homeService.GetScrapesAsync(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while searching scrapes.");
            }

            return PartialView("_Scrapes", scrapeResult);
        }
    }
}