using ConexaTest.Application.Queries.Users;
using ConexaTest.Domain.Errors.Users;
using ConexaTest.Domain.Models;
using ConexaTest.Infrastructure;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConexaTest.Application.Handlers.Users.Queries
{
    public class ValidateUserQueryHandler(AppDbContext dbContext) : IRequestHandler<ValidateUserQuery, ErrorOr<User>>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<ErrorOr<User>> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
            {
                return UserErrors.UserNotFound;
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return UserErrors.InvalidCredentials;
            }

            return user;
        }
    }
}
