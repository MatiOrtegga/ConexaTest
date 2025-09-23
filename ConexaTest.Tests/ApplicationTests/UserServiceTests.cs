using ConexaTest.Application.Commands.Users;
using ConexaTest.Application.Handlers.Users.Commands;
using ConexaTest.Application.Handlers.Users.Queries;
using ConexaTest.Application.Queries.Users;
using ConexaTest.Domain.Errors.Users;
using ConexaTest.Domain.Models;
using Xunit;

namespace ConexaTest.Tests.ApplicationTests
{
    public class UserServiceTest
    {

        [Fact]
        public async Task Should_Throw_Error_When_Email_Already_Exists()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new AddUserCommandHandler(fakeDbContext);

            var newRole = new Role { Name = "Admin" };
            await fakeDbContext.Roles.AddAsync(newRole);
            await fakeDbContext.SaveChangesAsync();


            fakeDbContext.Users.Add(
                new User
                {
                    Name = "Test User",
                    Email = "realGmail1234@gmail.com",
                    PasswordHash = "imagine this but hashed.",
                    RoleId = newRole.Id
                }
            );
            await fakeDbContext.SaveChangesAsync();

            var command = new AddUserCommand
            {
                Name = "Test User",
                Email = "realGmail1234@gmail.com",
                Password = "Password123!"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(UserErrors.EmailAlreadyInUse, result.FirstError);
        }
        [Fact]
        public async Task Should_Create_User_Successfully()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new AddUserCommandHandler(fakeDbContext);

            var newRole = new Role { Name = "User" };
            await fakeDbContext.Roles.AddAsync(newRole);
            await fakeDbContext.SaveChangesAsync();

            var command = new AddUserCommand
            {
                Name = "Test User",
                Email = "realGmail1234@gmail.com",
                Password = "Password123!",
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsError);
            Assert.True(result.Value);
        }
        [Fact]
        public async Task Should_Authenticate_User_Successfully()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new ValidateUserQueryHandler(fakeDbContext);

            var hasdPassword = BCrypt.Net.BCrypt.HashPassword("Password123!");
            var newRole = new Role { Name = "User" };
            await fakeDbContext.Roles.AddAsync(newRole);
            await fakeDbContext.SaveChangesAsync();

            var userToCreate = new User { Name = "Test User", Email = "realGmail1234@gmail.com", PasswordHash = hasdPassword, RoleId = newRole.Id };

            await fakeDbContext.Users.AddAsync(userToCreate);
            var userSaved = await fakeDbContext.SaveChangesAsync();

            Assert.Equal(1, userSaved);

            var query = new ValidateUserQuery
            {
                Email = "realGmail1234@gmail.com",
                Password = "Password123!"
            };
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsError);
            Assert.Equal(userToCreate.Email, result.Value.Email);
            Assert.Equal(userToCreate.Name, result.Value.Name);
        }
        [Fact]
        public async Task Should_Throw_Error_When_Password_Is_Incorrect()
        {
            var fakeDbContext = DbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var handler = new ValidateUserQueryHandler(fakeDbContext);

            var hasdPassword = BCrypt.Net.BCrypt.HashPassword("Password112312323!");

            var newRole = new Role { Name = "Admin" };
            await fakeDbContext.Roles.AddAsync(newRole);
            await fakeDbContext.SaveChangesAsync();

            var userToCreate = new User { Name = "TestUser", Email = "realGmail1234@gmail.com", PasswordHash = hasdPassword, RoleId = newRole.Id };

            await fakeDbContext.Users.AddAsync(userToCreate);
            var userSaved = await fakeDbContext.SaveChangesAsync();

            Assert.Equal(1, userSaved);

            var query = new ValidateUserQuery
            {
                Email = "realGmail1234@gmail.com",
                Password = "Password123!"
            };
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsError);
            Assert.Equal(UserErrors.InvalidCredentials, result.FirstError);

        }
    }
}