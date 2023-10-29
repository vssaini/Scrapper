using Scrapper.Application.Abstractions.Messaging;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Scrapes;

namespace Scrapper.Application.Scrapes.SearchScrapes;

internal sealed class SearchScrapesQueryHandler : IQueryHandler<SearchScrapesQuery, PageResult<ScrapeResponse>>
{
    private readonly IScrapeRepository _scrapeRepo;

    public SearchScrapesQueryHandler(IScrapeRepository scrapeRepo)
    {
        _scrapeRepo = scrapeRepo;
    }

    public async Task<PageResult<ScrapeResponse>> Handle(SearchScrapesQuery query, CancellationToken cancellationToken)
    {
        var pageResult = await _scrapeRepo.GetScrapePageResultAsync(query.Filter, query.Page, query.Sort);
        return pageResult;
    }
}