using ConexaTest.Application.Commands.Users;
using ConexaTest.Domain.Errors.Users;
using ConexaTest.Domain.Models;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConexaTest.Application.Handlers.Users.Commands
{
    public class AddUserCommandHandler(AppDbContext dbContext) : IRequestHandler<AddUserCommand, ErrorOr<bool>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<ErrorOr<bool>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var userWithSameEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (userWithSameEmail != null)
            {
                return UserErrors.EmailAlreadyInUse;
            }
            if (request.RoleId == 0)
            {
                return UserErrors.RoleNotFound;
            }
            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = request.RoleId
            };

            _dbContext.Users.Add(newUser);
            var response = await _dbContext.SaveChangesAsync(cancellationToken);

            if (response == 0)
            {
                return UserErrors.CantCreateUser;
            }
            return true;
        }
    }
}
