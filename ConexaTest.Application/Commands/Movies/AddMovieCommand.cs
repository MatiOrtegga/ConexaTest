using ConexaTest.Domain.Dto;
using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Commands.Movies
{
    public class AddMovieCommand : AddMovieDto, IRequest<ErrorOr<bool>>
    {        
    }
}
