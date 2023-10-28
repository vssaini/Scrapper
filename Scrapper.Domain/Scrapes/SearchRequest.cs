namespace Scrapper.Domain.Scrapes;

public record SearchRequest(bool IsForeignStatement, DateRange DateRange, int AccountNumber, Pagination Pagination);