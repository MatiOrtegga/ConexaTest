using ErrorOr;
using System.Diagnostics.CodeAnalysis;

namespace ConexaTest.Domain.Errors.Movies
{
    
    public static class MoviesError
    {
        public static Error NoMovieFound => Error.NotFound(
            code: "Movies.NoMovieFound",
            description: "No movie found with the provided criteria."
        );

        public static Error MovieAlreadyExists => Error.Conflict(
            code: "Movies.MovieAlreadyExists",
            description: "A movie with the same title and director already exists."
        );
        public static Error CantAddMovie => Error.Failure(
            code: "Movies.CantAddMovie",
            description: "Failed to add movie."
        );  
        public static Error CantUpdateMovie => Error.Failure(
            code: "Movies.CantUpdateMovie",
            description: "Failed to update movie."
        );
        public static Error CantDeleteMovie => Error.Failure(
            code: "Movies.CantDeleteMovie",
            description: "Failed to delete movie."
        );
    }
}
