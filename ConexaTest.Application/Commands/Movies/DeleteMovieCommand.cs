using ErrorOr;
using MediatR;
namespace ConexaTest.Application.Commands.Movies
{
    public class DeleteMovieCommand : IRequest<ErrorOr<bool>>
    {
        public int Id { get; set; }
    }
}
