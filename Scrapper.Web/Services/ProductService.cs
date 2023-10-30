using MediatR;
using Scrapper.Application.Products.ProductLogs;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Products;
using Scrapper.Web.Contracts;

namespace Scrapper.Web.Services;

public class ProductService : IProductService
{
    private readonly ISender _sender;

    public ProductService(ISender sender)
    {
        _sender = sender;
    }

    // TODO: Add sorting to request, both in Home and Product
    public async Task<Product> GetProductAsync(ProductRequest request)
    {
        var pageResult = await GetProductLogsAsync(request);

        var productName = pageResult.Items.Select(i => i.ProductName).FirstOrDefault();
        return new Product(request.ProductId,productName, pageResult);
    }

    public async Task<PageResult<ProductResponse>> GetProductLogsAsync(ProductRequest request)
    {
        var filter = new ProductFilter(request.ProductId);
        var query = new ProductLogsQuery(filter,
            new Page(request.Page.Number, request.Page.Size),
            new Sort("BatchDate", "DESC"));

        var pageResult = await _sender.Send(query);

        return pageResult;
    }
}