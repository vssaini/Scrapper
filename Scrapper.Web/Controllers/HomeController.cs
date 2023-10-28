using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Royalties;
using Scrapper.Models;
using System.Diagnostics;

namespace Scrapper.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ISender _sender;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ISender sender, ILogger<HomeController> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var pageResult = new PageResult<ScrapeResponse>();

            try
            {
                // TODO: Get default search request
                // TODO: Get all paginated scrapes from home service using sender

                //var filter = new SearchFilter(request.DateRange, request.AccountNumber);
                //var query = new SearchRoyaltiesQuery(filter,
                //new Pagination(request.Pagination.PageNumber, request.Pagination.PageSize),
                //new Sort("ID", "ASC"));

                //var pageResult = await _sender.Send(query);
                //return Ok(pageResult);

                // var userName = HttpContext.User.Identity.Name;
                // pageResult = await _homeService.GetScrappedResults();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving scrapes.");
            }

            return View(pageResult);
        }

        //[HttpPost]
        //public async Task<IActionResult> SearchRoyalties(SearchRequest request)
        //{
        //    var rangeTxt = request.IsForeignStatement ? "Foreign Statement" : "Session";

        //    _logger.LogInformation("Searching royalties for account {AccountNumber} via {RangeFilter} filter. Start date - {StartDate} & End date - {EndDate}. Page number - {PageNumber} & Page size - {PageSize}.", request.AccountNumber, rangeTxt, request.DateRange.Start, request.DateRange.End, request.Pagination.PageNumber, request.Pagination.PageSize);

        //    var filter = new SearchFilter(request.DateRange, request.AccountNumber);
        //    var query = new SearchRoyaltiesQuery(filter,
        //        new Pagination(request.Pagination.PageNumber, request.Pagination.PageSize),
        //        new Sort("ID", "ASC"));

        //    var pageResult = await _sender.Send(query);
        //    return Ok(pageResult);
        //}

        //[HttpPost]
        //public async Task<IActionResult> SearchRoyalties([FromBody] SearchRequest request)
        //{
        //    var result = new PageResult<RoyaltyResponse>();

        //    try
        //    {
        //        result = await _homeService.GetRoyaltiesPageResultAsync(request);

        //        result.Page = request.Pagination.PageNumber;
        //    }
        //    catch (Exception e)
        //    {
        //        _logService.LogError(e, "Error while searching royalties.");
        //    }

        //    return PartialView("_Royalties", result);
        //}

        //public async Task<IActionResult> DownloadPdf([FromQuery] DocRequest request)
        //{
        //    var document = await _homeService.GetDocumentAsync(request);

        //    var docBytes = document.GeneratePdf();
        //    Stream stream = new MemoryStream(docBytes);

        //    return new FileStreamResult(stream, "application/pdf")
        //    {
        //        FileDownloadName = document.FileName
        //    };
        //}

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}