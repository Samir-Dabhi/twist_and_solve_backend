using FluentValidation;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Validators
{
    public class FeedbackValidator : AbstractValidator<FeedbackModel>
    {
        public FeedbackValidator()
        {
            RuleFor(f => f.FeedbackId)
                .GreaterThan(0)
                .WithMessage("FeedbackId must be greater than 0.");

            RuleFor(f => f.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0.");

            RuleFor(f => f.LessonId)
                .GreaterThan(0)
                .When(f => f.LessonId.HasValue)
                .WithMessage("LessonId, if provided, must be greater than 0.");

            RuleFor(f => f.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");

            RuleFor(f => f.Comment)
                .MaximumLength(1000)
                .WithMessage("Comment must not exceed 1000 characters.");

            RuleFor(f => f.FeedbackDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("FeedbackDate cannot be in the future.");
        }
    }
}
