using ConexaTest.Application.Queries.Users;
using FluentValidation;

namespace ConexaTest.Application.Validators.Users
{
    public class ValidateUserValidator : AbstractValidator<ValidateUserQuery>
    {
        public ValidateUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
