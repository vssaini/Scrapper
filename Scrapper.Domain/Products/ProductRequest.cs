using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Products;

public record ProductRequest(string ProductId, Page Pagination);