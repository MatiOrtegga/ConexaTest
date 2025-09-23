using ErrorOr;

namespace ConexaTest.Domain.Errors.Users
{
    public static class UserErrors
    {
        public static Error UserNotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found."
        );
        public static Error InvalidCredentials => Error.Validation(
            code: "User.InvalidCredentials",
            description: "Invalid credentials."
        );
        public static Error EmailAlreadyInUse => Error.Conflict(
            code: "User.EmailAlreadyInUse",
            description: "Email is already in use."
        );

        public static Error CantCreateUser => Error.Failure(
            code: "User.CantCreateUser",
            description: "Failed to create user."
        );
        public static Error RoleNotFound => Error.NotFound(
            code: "Role.NotFound",
            description: "Role not found."
        );
    }
}
