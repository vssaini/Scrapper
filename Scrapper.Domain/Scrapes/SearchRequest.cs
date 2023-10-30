using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Scrapes;

public record SearchRequest(DateRange DateRange, string SearchText, Page Page, Sort Sort);