using Scrapper.Domain.Abstractions;
using Scrapper.Domain.Scrapes;
using Scrapper.Web.Contracts;
using System.Text.Json;

namespace Scrapper.Web.Services;

public class HomeService : IHomeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<PageResult<ScrapeResponse>> GetScrapesAsync(SearchRequest searchRequest)
    {
        searchRequest ??= GetDefaultSearchRequest();

        const string path = "api/royalties";
        var httpClient = _httpClientFactory.CreateClient(""); // TODO: Get rid of such code

        var json = JsonSerializer.Serialize(searchRequest);
        var content = new StringContent(json, null, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(path, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PageResult<ScrapeResponse>>();
    }

    private static SearchRequest GetDefaultSearchRequest()
    {
        // We can't use DateTime.MinValue because it's not supported by SQL Server
        var minStartDate = new DateTime(1753, 1, 1);

        var dateRange = new DateRange(minStartDate, DateTime.Now);
        return new SearchRequest(true, dateRange, 0, new Pagination(1, 10));
    }
}