using ConexaTest.Domain.Dto;
using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Commands.Users
{
    public class AddUserCommand : AddUserDto, IRequest<ErrorOr<bool>>
    {
    }
}
