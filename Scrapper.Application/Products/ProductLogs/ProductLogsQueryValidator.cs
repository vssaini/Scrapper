using FluentValidation;

namespace Scrapper.Application.Products.ProductLogs;

public class ProductLogsQueryValidator : AbstractValidator<ProductLogsQuery>
{
    public ProductLogsQueryValidator()
    {
        //RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
    }
}