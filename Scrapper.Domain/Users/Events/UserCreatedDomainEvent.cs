using Scrapper.Domain.Abstractions;

namespace Scrapper.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(UserId Id):IDomainEvent;