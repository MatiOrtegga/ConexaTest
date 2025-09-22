using ConexaTest.Domain.Models;
using ErrorOr;
using MediatR;


namespace ConexaTest.Application.Queries.Movies
{
    public class GetMovieByIdQuery : IRequest<ErrorOr<Movie>>
    {
        public int Id { get; set; }
    }
}
