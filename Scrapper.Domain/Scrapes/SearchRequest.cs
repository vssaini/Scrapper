﻿using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Scrapes;

public record SearchRequest(DateRange DateRange, string SearchText, Pagination Pagination);