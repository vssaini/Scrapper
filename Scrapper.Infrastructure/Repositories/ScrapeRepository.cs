using Dapper;
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

    public async Task<PageResult<ScrapeResponse>> GetScrapePageResultAsync(SearchFilter filter, Page page, Sort sort)
    {
        var dParams = GetParamsForSearchSp(filter, page, sort);
        var pageResult = await SearchScrapesAsync(dParams, page);
        return pageResult;
    }

    private static DynamicParameters GetParamsForSearchSp(SearchFilter filter, Page page, Sort sort)
    {
        var dParams = new DynamicParameters();
        dParams.Add("@StartDate", filter.DateRange.Start);
        dParams.Add("@EndDate", filter.DateRange.End);
        dParams.Add("@SearchText", filter.SearchText);

        dParams.Add("@SortColumn", sort.Column);
        dParams.Add("@SortDirection", sort.Direction);
        dParams.Add("@PageNumber", page.Number);
        dParams.Add("@PageSize", page.Size);

        return dParams;
    }

    private async Task<PageResult<ScrapeResponse>> SearchScrapesAsync(SqlMapper.IDynamicParameters dParams, Page page)
    {
        var scrapes = await GetScrapesFromDbAsync(dParams);

        return new PageResult<ScrapeResponse>
        {
            Items = scrapes,
            Page = page.Number,
            PageSize = page.Size,
            TotalItems = scrapes.Count > 0 ? scrapes.Select(x => x.TotalRows).First() : 0
        };
    }

    private async Task<List<ScrapeResponse>> GetScrapesFromDbAsync(SqlMapper.IDynamicParameters dParams)
    {
        const string spName = "dbo.usp_SearchScrapes";
        var command = new CommandDefinition(spName, dParams, commandType: CommandType.StoredProcedure);

        await using var connection = _dbContext.Database.GetDbConnection();

        var response = await connection.QueryAsync<ScrapeResponse>(command);
        return response.ToList();
    }
}