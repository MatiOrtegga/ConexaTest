using ErrorOr;

namespace ConexaTest.Domain.Errors.Swapi
{
    public static class SwapiErrors
    {
        public static Error SwapiServiceUnavailable => Error.Failure(
            code: "Swapi.ServiceUnavailable",
            description: "SWAPI service is currently unavailable."
        );
        public static Error SwapiMovieNotFound => Error.NotFound(
            code: "Swapi.MovieNotFound",
            description: "Star Wars movie not found in SWAPI."
        );
        public static Error SwapiInvalidResponse => Error.Failure(
            code: "Swapi.InvalidResponse",
            description: "Received an invalid response from SWAPI."
        );
    }
}
