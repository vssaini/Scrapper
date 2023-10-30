using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Scrapes;

public record ScrapeResult(PageResult<Scrape> Scrapes, Page Page, Sort Sort);