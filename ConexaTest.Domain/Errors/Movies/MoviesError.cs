using ErrorOr;

namespace ConexaTest.Domain.Errors.Movies
{
    public static class MoviesError
    {
        public static Error NoMovieFound => Error.NotFound(
            code: "Movies.NoMovieFound",
            description: "No movie found with the provided criteria."
        );
    }
}
