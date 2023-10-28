namespace Scrapper.Domain.Scrapes;

public record DocRequest(bool IsForeignStatement, DateRange DateRange, int AccountNumber);