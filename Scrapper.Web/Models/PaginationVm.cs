namespace Scrapper.Web.Models;

public record PaginationVm(int TotalItemCount, int PageNumber, int PageCount, int PageSize);