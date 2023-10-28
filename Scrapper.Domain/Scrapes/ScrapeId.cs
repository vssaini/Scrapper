namespace Scrapper.Domain.Scrapes;

public record ScrapeId(Guid Value)
{
    public static ScrapeId New() => new(Guid.NewGuid());
}