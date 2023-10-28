namespace Scrapper.Domain.Royalties;

public record DocRequest(bool IsForeignStatement, DateRange DateRange, int AccountNumber);