using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Royalties;

public interface IScrapeRepository
{
    Task<PageResult<ScrapeResponse>> GetScrapePageResultAsync(SearchFilter filter, Pagination page, Sort sort);
}