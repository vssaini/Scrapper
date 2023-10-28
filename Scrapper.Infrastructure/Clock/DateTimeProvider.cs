using Scrapper.Application.Abstractions.Clock;

namespace Scrapper.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}