using FluentValidation;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0.");

            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MaximumLength(50)
                .WithMessage("Username must not exceed 50 characters.")
                .Matches("^[a-zA-Z0-9_]+$")
                .WithMessage("Username can only contain letters, numbers, and underscores.");

            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email must be a valid email address.");

            RuleFor(u => u.PasswordHash)
                .NotEmpty()
                .WithMessage("PasswordHash is required.")
                .MinimumLength(64)
                .WithMessage("PasswordHash must be at least 64 characters long.")
                .MaximumLength(64)
                .WithMessage("PasswordHash must not exceed 64 characters."); // Assuming SHA-256 hash length.

            RuleFor(u => u.DateJoined)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("DateJoined cannot be in the future.");

            RuleFor(u => u.ProfilePicture)
                .NotEmpty()
                .WithMessage("ProfilePicture is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("ProfilePicture must be a valid URL.");

            RuleFor(u => u.ProgressLevel)
                .GreaterThanOrEqualTo(0)
                .WithMessage("ProgressLevel must be 0 or greater.");
        }
    }
}
