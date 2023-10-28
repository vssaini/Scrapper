namespace Scrapper.Web.Models.Exceptions;

public sealed record ValidationError(string PropertyName, string ErrorMessage);