using FluentValidation;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Validators
{
    public class AchievementValidator : AbstractValidator<AchievementModel>
    {
        public AchievementValidator()
        {
            RuleFor(a => a.AchievementId)
                .GreaterThan(0)
                .WithMessage("AchievementId must be greater than 0.");

            RuleFor(a => a.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(100)
                .WithMessage("Title must not exceed 100 characters.");

            RuleFor(a => a.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

            RuleFor(a => a.IconUrl)
                .NotEmpty()
                .WithMessage("IconUrl is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("IconUrl must be a valid URL.");
        }
    }
}
