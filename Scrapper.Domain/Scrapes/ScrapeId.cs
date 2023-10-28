namespace Scrapper.Domain.Royalties;

public record ScrapeId(Guid Value)
{
    public static ScrapeId New() => new(Guid.NewGuid());
}