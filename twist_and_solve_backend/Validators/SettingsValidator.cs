using FluentValidation;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Validators
{
    public class SettingsValidator : AbstractValidator<SettingsModel>
    {
        public SettingsValidator()
        {
            RuleFor(s => s.SettingId)
                .GreaterThan(0)
                .WithMessage("SettingId must be greater than 0.");

            RuleFor(s => s.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0.");

            RuleFor(s => s.Language)
                .NotEmpty()
                .WithMessage("Language is required.")
                .MaximumLength(10)
                .WithMessage("Language must not exceed 10 characters.")
                .Matches("^[a-zA-Z-]+$")
                .WithMessage("Language must only contain letters and hyphens (e.g., 'en', 'en-US').");
        }
    }
}
