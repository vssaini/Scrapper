using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Scrapes;

namespace Scrapper.Web.Contracts;

public interface IHomeService
{
    Task<ScrapeResult> GetScrapesAsync(SearchRequest searchRequest);
}