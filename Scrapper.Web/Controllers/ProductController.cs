using Microsoft.AspNetCore.Mvc;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Products;
using Scrapper.Web.Contracts;

namespace Scrapper.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _prodService;

        public ProductController(ILogger<ProductController> logger, IProductService prodService)
        {
            _logger = logger;
            _prodService = prodService;
        }

        public async Task<IActionResult> Index(string productId)
        {
            var product = new Product(productId, "NA", new PageResult<ProductResponse>());

            try
            {
                product = await _prodService.GetProductAsync(new ProductRequest(productId, new Page(1, 10)));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving product with id {ProductId} logs.", productId);
            }

            return View(product);
        }

        public async Task<IActionResult> Logs([FromBody] ProductRequest request)
        {
            var productLogs = new PageResult<ProductResponse>();

            try
            {
                productLogs = await _prodService.GetProductLogsAsync(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving product logs with id {ProductId} logs.", request.ProductId);
            }

            return PartialView("_Logs", productLogs);
        }
    }
}
