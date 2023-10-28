﻿using Dapper;
using Microsoft.EntityFrameworkCore;
using Scrapper.Domain.Abstractions;
using System.Data;
using Scrapper.Domain.Scrapes;

namespace Scrapper.Infrastructure.Repositories;

internal sealed class ScrapeRepository : IScrapeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ScrapeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PageResult<ScrapeResponse>> GetScrapePageResultAsync(SearchFilter filter, Pagination page, Sort sort)
    {
        var dParams = GetParamsForSearchSp(filter, page, sort);
        var pageResult = await SearchScrapesAsync(dParams, page);
        return pageResult;
    }

    private static DynamicParameters GetParamsForSearchSp(SearchFilter filter, Pagination page, Sort sort)
    {
        var dParams = new DynamicParameters();
        dParams.Add("@StartDate", filter.DateRange.Start);
        dParams.Add("@EndDate", filter.DateRange.End);
        dParams.Add("@ProductId", string.Empty);
        dParams.Add("@SortOrder", sort.SortOrder);
        dParams.Add("@SortMethod", sort.SortMethod);
        dParams.Add("@PageNumber", page.PageNumber);
        dParams.Add("@PageSize", page.PageSize);

        return dParams;
    }

    private async Task<PageResult<ScrapeResponse>> SearchScrapesAsync(SqlMapper.IDynamicParameters dParams, Pagination page)
    {
        var scrapes = await GetScrapesFromDbAsync(dParams);

        return new PageResult<ScrapeResponse>
        {
            Items = scrapes,
            Page = page.PageNumber,
            PageSize = page.PageSize,
            TotalItems = scrapes.Count > 0 ? scrapes.Select(x => x.TotalRows).First() : 0
        };
    }

    private async Task<List<ScrapeResponse>> GetScrapesFromDbAsync(SqlMapper.IDynamicParameters dParams)
    {
        const string spName = "dbo.usp_SearchScrapes";
        var command = new CommandDefinition(spName, dParams, commandType: CommandType.StoredProcedure);

        await using var connection = _dbContext.Database.GetDbConnection();

        await using var gr = await connection.QueryMultipleAsync(command);
        var scrapes = gr.Read<ScrapeResponse>().ToList();

        return scrapes;
    }
}