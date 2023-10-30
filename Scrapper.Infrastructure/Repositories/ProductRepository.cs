using Dapper;
using Microsoft.EntityFrameworkCore;
using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Products;
using System.Data;

namespace Scrapper.Infrastructure.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PageResult<ProductResponse>> GetProductLogsAsync(ProductFilter filter, Page page, Sort sort)
    {
        var dParams = GetParamsForSearchSp(filter, page, sort);
        var pageResult = await GetProductLogsAsync(dParams, page);
        return pageResult;
    }

    private static DynamicParameters GetParamsForSearchSp(ProductFilter filter, Page page, Sort sort)
    {
        var dParams = new DynamicParameters();
        dParams.Add("@ProductId", filter.ProductId);

        dParams.Add("@SortColumn", sort.Column);
        dParams.Add("@SortDirection", sort.Direction);
        dParams.Add("@PageNumber", page.Number);
        dParams.Add("@PageSize", page.Size);

        return dParams;
    }

    private async Task<PageResult<ProductResponse>> GetProductLogsAsync(SqlMapper.IDynamicParameters dParams, Page page)
    {
        var productLogs = await GetProductLogsFromDbAsync(dParams);

        return new PageResult<ProductResponse>
        {
            Items = productLogs,
            Page = page.Number,
            PageSize = page.Size,
            TotalItems = productLogs.Count > 0 ? productLogs.Select(x => x.TotalRows).First() : 0
        };
    }

    private async Task<List<ProductResponse>> GetProductLogsFromDbAsync(SqlMapper.IDynamicParameters dParams)
    {
        const string spName = "dbo.usp_GetProductLogs";
        var command = new CommandDefinition(spName, dParams, commandType: CommandType.StoredProcedure);

        await using var connection = _dbContext.Database.GetDbConnection();

        var response = await connection.QueryAsync<ProductResponse>(command);
        return response.ToList();
    }
}