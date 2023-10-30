using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Scrapes;

namespace Scrapper.Domain.Products;

public interface IProductRepository
{
    Task<PageResult<ProductResponse>> GetProductLogsAsync(ProductFilter filter, Page page, Sort sort);
}