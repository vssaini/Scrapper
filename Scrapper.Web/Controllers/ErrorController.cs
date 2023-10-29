using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scrapper.Web.Controllers
{
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorController : Controller
    {
        // Best solution - https://stackoverflow.com/a/30072933

        [Route("500")]
        public ActionResult Page500()
        {
            Response.StatusCode = 500;
            return View();
        }

        [Route("404")]
        public IActionResult Page404()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}