using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Commands.Movies
{
    public class SyncSwapMoviesCommand : IRequest<ErrorOr<bool>>
    {
    }
}
