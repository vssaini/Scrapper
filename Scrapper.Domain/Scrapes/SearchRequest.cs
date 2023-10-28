namespace Scrapper.Domain.Royalties;

public record SearchRequest(bool IsForeignStatement, DateRange DateRange, int AccountNumber, Pagination Pagination);