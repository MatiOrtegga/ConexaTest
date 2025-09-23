using ConexaTest.Application.Commands.Movies;
using FluentValidation;

namespace ConexaTest.Application.Validators.Movies
{
    public class DeleteMovieValidator : AbstractValidator<DeleteMovieCommand>
    {
        public DeleteMovieValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
