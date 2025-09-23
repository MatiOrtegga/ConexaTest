using ConexaTest.Domain.Models;
using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Queries.Users
{
    public class ValidateUserQuery : IRequest<ErrorOr<User>>
    {
        public string? Email { get; set; } 
        public string? Password { get; set; } 
    }
}
