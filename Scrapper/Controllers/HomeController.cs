using Microsoft.AspNetCore.Mvc;
using Scrapper.Models;
using System.Diagnostics;
using Scrapper.Contracts;

namespace Scrapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TakeScreenshot([FromServices] IWescoService wescoService)
        {
            var result = new Result<bool>() { Type = ResponseType.Ok };

            try
            {
                await wescoService.TakeScreenshotAsync();

                result.SuccessMessage = "Screenshot taken successfully!";
                result.Data = true;
            }
            catch (Exception e)
            {
                result.Type = ResponseType.Error;
                result.ErrorMessage = e.Message;
            }

            return Json(result);
        }

        public IActionResult Privacy() => View();


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}