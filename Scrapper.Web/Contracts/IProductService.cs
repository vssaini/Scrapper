using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Products;

namespace Scrapper.Web.Contracts;

public interface IProductService
{
    Task<Product> GetProductAsync(ProductRequest request);
    Task<PageResult<ProductResponse>> GetProductLogsAsync(ProductRequest request);
}