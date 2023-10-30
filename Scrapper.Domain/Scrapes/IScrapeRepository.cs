using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Scrapes;

public interface IScrapeRepository
{
    Task<PageResult<Scrape>> GetScrapePageResultAsync(SearchFilter filter, Page page, Sort sort);
}