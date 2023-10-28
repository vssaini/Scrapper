namespace Scrapper.Contracts.Clock;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}