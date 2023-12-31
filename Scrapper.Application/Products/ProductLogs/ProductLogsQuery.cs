﻿using Scrapper.Application.Abstractions.Messaging;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Products;

namespace Scrapper.Application.Products.ProductLogs;

public sealed record ProductLogsQuery(ProductFilter Filter, Page Page, Sort Sort) : IQuery<PageResult<ProductResponse>>;