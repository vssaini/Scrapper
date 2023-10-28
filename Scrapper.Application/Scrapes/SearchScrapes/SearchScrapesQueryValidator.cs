using FluentValidation;

namespace Scrapper.Application.Scrapes.SearchRoyalties;

public class SearchScrapesQueryValidator : AbstractValidator<SearchScrapesQuery>
{
    public SearchScrapesQueryValidator()
    {
        //RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
    }
}