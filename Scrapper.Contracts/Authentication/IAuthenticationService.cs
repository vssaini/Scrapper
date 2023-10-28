using Scrapper.Domain.Users;

namespace Scrapper.Contracts.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default);
}