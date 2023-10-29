using Scrapper.Application.Abstractions.Messaging;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Products;

namespace Scrapper.Application.Products.ProductLogs;

internal sealed class ProductLogsQueryHandler : IQueryHandler<ProductLogsQuery, PageResult<ProductResponse>>
{
    private readonly IProductRepository _prodRepo;

    public ProductLogsQueryHandler(IProductRepository royRepo)
    {
        _prodRepo = royRepo;
    }

    public async Task<PageResult<ProductResponse>> Handle(ProductLogsQuery query, CancellationToken cancellationToken)
    {
        var pageResult = await _prodRepo.GetProductLogsAsync(query.Filter, query.Page, query.Sort);
        return pageResult;
    }
}