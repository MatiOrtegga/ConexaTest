using ConexaTest.Application.Commands.Movies;
using FluentValidation;

namespace ConexaTest.Application.Validators.Movies
{
    public class AddMovieDtoValidator : AbstractValidator<AddMovieCommand>
    {
        public AddMovieDtoValidator()
        {
            RuleFor(x => x.ExternalId)
                .NotEmpty().WithMessage("ExternalId is required.")
                .MaximumLength(50).WithMessage("ExternalId must not exceed 50 characters.");

            RuleFor(x => x.Source)
                .NotEmpty().WithMessage("Source is required.")
                .MaximumLength(30).WithMessage("Source must not exceed 30 characters.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Director)
                .NotEmpty().WithMessage("Director is required.")
                .MaximumLength(100).WithMessage("Director must not exceed 100 characters.");

            RuleFor(x => x.Producer)
                .NotEmpty().WithMessage("Producer is required.")
                .MaximumLength(100).WithMessage("Producer must not exceed 100 characters.");

            RuleFor(x => x.ReleaseDate)
                .NotNull().WithMessage("Release date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Release date cannot be in the future.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.EpisodeId)
                .NotNull().WithMessage("EpisodeId is required.")
                .GreaterThan(0).WithMessage("EpisodeId must be greater than 0.");
        }
    }
}