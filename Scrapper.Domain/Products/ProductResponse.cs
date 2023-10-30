namespace Scrapper.Domain.Products;

public sealed class ProductResponse
{
    public int Batch { get; init; }

    public DateTime BatchDate { get; init; }

    public string ProductName { get; init; }

    public decimal ProductPrice { get; init; }

    public int TotalRows { get; set; }
}