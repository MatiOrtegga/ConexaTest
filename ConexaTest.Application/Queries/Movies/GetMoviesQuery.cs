using ConexaTest.Domain.Models;
using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Queries.Movies
{
    public class GetMoviesQuery : IRequest<ErrorOr<IEnumerable<Movie>>>
    {
    }
}
