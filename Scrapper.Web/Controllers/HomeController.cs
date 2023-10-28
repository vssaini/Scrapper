﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Scrapes;
using Scrapper.Web.Contracts;
using Scrapper.Web.Models;
using System.Diagnostics;

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
            var pageResult = new PageResult<ScrapeResponse>();

            try
            {
                pageResult = await _homeService.GetScrapesAsync(null);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving scrapes.");
            }

            return View(pageResult);
        }

        [HttpPost]
        public async Task<IActionResult> SearchScrapes(SearchRequest request)
        {
            var pageResult = new PageResult<ScrapeResponse>();

            var rangeTxt = request.IsForeignStatement ? "Foreign Statement" : "Session";

            _logger.LogInformation("Searching scrapes for account {AccountNumber} via {RangeFilter} filter. Start date - {StartDate} & End date - {EndDate}. Page number - {PageNumber} & Page size - {PageSize}.", request.AccountNumber, rangeTxt, request.DateRange.Start, request.DateRange.End, request.Pagination.PageNumber, request.Pagination.PageSize);

            try
            {
                pageResult = await _homeService.GetScrapesAsync(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while searching scrapes.");
            }

            return Ok(pageResult);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}