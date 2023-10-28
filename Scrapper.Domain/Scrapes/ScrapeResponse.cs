namespace Scrapper.Domain.Scrapes;

public sealed class ScrapeResponse
{
    public int Id { get; init; }

    public int Batch { get; init; }

    public DateTime BatchDate { get; init; }

    public string ProductId { get; init; }

    public string ProductName { get; init; }

    public string ProductPrice { get; init; }

    public int TotalRows { get; set; }
}