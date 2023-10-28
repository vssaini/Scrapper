namespace Scrapper.Domain.Scrapes;

public record SearchRequest(DateRange DateRange, string ProductId, Pagination Pagination);