using FluentValidation;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Validators
{
    public class LessonValidator : AbstractValidator<LessonModel>
    {
        public LessonValidator()
        {
            RuleFor(l => l.LessonId)
                .GreaterThan(0)
                .WithMessage("LessonId must be greater than 0.");

            RuleFor(l => l.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(200)
                .WithMessage("Title must not exceed 200 characters.");

            RuleFor(l => l.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(1000)
                .WithMessage("Description must not exceed 1000 characters.");

            RuleFor(l => l.StepOrder)
                .GreaterThan(0)
                .WithMessage("StepOrder must be greater than 0.");

            RuleFor(l => l.ImageUrl)
                .NotEmpty()
                .WithMessage("ImageUrl is required.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("ImageUrl must be a valid URL.");
        }
    }
}
