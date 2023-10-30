namespace Scrapper.Domain.Scrapes;

public sealed class Scrape
{
    public int Id { get; init; }

    public DateTime BatchDate { get; init; }

    public string ProductId { get; init; }

    public string ProductName { get; init; }

    public string MaxPrice { get; init; }

    public string MinPrice { get; init; }

    public int TotalRows { get; set; }
}