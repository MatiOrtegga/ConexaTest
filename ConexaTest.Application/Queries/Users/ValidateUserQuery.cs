using ConexaTest.Domain.Models;
using ErrorOr;
using MediatR;

namespace ConexaTest.Application.Queries.Users
{
    public class ValidateUserQuery : IRequest<ErrorOr<User>>
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
