
using ConexaTest.Application.Queries.Movies;
using FluentValidation;

namespace ConexaTest.Application.Validators.Movies
{
    public class GetMovieByIdValidator : AbstractValidator<GetMovieByIdQuery>
    {
        public GetMovieByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
