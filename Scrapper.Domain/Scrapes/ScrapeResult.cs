using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Scrapes;

public record ScrapeResult(PageResult<ScrapeResponse> Scrapes, Page Page, Sort Sort);