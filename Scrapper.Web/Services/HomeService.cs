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

    public async Task<ScrapeResult> GetScrapesAsync(SearchRequest? request)
    {
        request ??= GetDefaultSearchRequest();

        var filter = new SearchFilter(request.DateRange, request.SearchText);
        var query = new SearchScrapesQuery(filter, request.Page, request.Sort);

        var pageResult = await _sender.Send(query);
        return new ScrapeResult(pageResult, request.Page, request.Sort);
    }

    private static SearchRequest GetDefaultSearchRequest()
    {
        // We can't use DateTime.MinValue because it's not supported by SQL Server
        var minStartDate = new DateTime(1753, 1, 1);
        var dateRange = new DateRange(minStartDate, DateTime.Now);

        var pagination = new Page(1, 10);
        var sort = new Sort("BatchDate", "DESC");

        return new SearchRequest(dateRange, null, pagination, sort);
    }
}