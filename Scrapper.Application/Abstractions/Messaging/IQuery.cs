using MediatR;

namespace Scrapper.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}