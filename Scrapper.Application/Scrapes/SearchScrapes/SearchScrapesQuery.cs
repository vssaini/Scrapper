using Scrapper.Application.Abstractions.Messaging;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Royalties;

namespace Scrapper.Application.Scrapes.SearchRoyalties;

public sealed record SearchScrapesQuery(SearchFilter Filter, Pagination Page, Sort Sort) : IQuery<PageResult<ScrapeResponse>>;