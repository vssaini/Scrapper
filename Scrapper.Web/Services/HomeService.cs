using MediatR;
using Scrapper.Application.Scrapes.SearchScrapes;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Scrapes;
using Scrapper.Web.Contracts;

namespace Scrapper.Web.Services;

public class HomeService : IHomeService
{
    private readonly ISender _sender;

    public HomeService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<PageResult<ScrapeResponse>> GetScrapesAsync(SearchRequest? request)
    {
        request ??= GetDefaultSearchRequest();

        var filter = new SearchFilter(request.DateRange, request.ProductId);
        var query = new SearchScrapesQuery(filter,
            new Pagination(request.Pagination.PageNumber, request.Pagination.PageSize),
            new Sort("Id", "ASC"));

        var pageResult = await _sender.Send(query);
        return pageResult;
    }

    private static SearchRequest GetDefaultSearchRequest()
    {
        // We can't use DateTime.MinValue because it's not supported by SQL Server
        var minStartDate = new DateTime(1753, 1, 1);

        var dateRange = new DateRange(minStartDate, DateTime.Now);
        return new SearchRequest(dateRange, null, new Pagination(1, 10));
    }
}