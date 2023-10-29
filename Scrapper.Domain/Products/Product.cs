using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Products;

public record Product(string ProductId, string ProductName, PageResult<ProductResponse> Logs);