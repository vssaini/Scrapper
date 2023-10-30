namespace Scrapper.Domain.Scrapes;

public sealed class Scrape
{
    public int Id { get; init; }

    public DateTime BatchDate { get; init; }

    public string ProductId { get; init; }

    public string ProductName { get; init; }

    public decimal MaxPrice { get; init; }

    public decimal MinPrice { get; init; }

    public int ProductStock { get; set; }

    public int TotalRows { get; set; }
}