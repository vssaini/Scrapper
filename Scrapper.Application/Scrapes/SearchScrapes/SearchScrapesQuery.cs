using Scrapper.Application.Abstractions.Messaging;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Scrapes;

namespace Scrapper.Application.Scrapes.SearchScrapes;

public sealed record SearchScrapesQuery(SearchFilter Filter, Pagination Page, Sort Sort) : IQuery<PageResult<ScrapeResponse>>;