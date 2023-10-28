using Scrapper.Application.Abstractions.Messaging;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Scrapes;

namespace Scrapper.Application.Scrapes.SearchScrapes;

internal sealed class SearchScrapesQueryHandler : IQueryHandler<SearchScrapesQuery, PageResult<ScrapeResponse>>
{
    private readonly IScrapeRepository _royRepo;

    public SearchScrapesQueryHandler(IScrapeRepository royRepo)
    {
        _royRepo = royRepo;
    }

    public async Task<PageResult<ScrapeResponse>> Handle(SearchScrapesQuery request, CancellationToken cancellationToken)
    {
        var pageResult = await _royRepo.GetScrapePageResultAsync(request.Filter, request.Page, request.Sort);
        return pageResult;
    }
}