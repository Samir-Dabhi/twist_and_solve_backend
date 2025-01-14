using FluentValidation;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Validators
{
    public class AlgorithmValidator : AbstractValidator<AlgorithmModel>
    {
        public AlgorithmValidator()
        {
            RuleFor(a => a.AlgorithmId)
                .GreaterThan(0)
                .WithMessage("AlgorithmId must be greater than 0.");

            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(a => a.Notation)
                .NotEmpty()
                .WithMessage("Notation is required.")
                .Matches("^[RUFBLD'2 ]+$")
                .WithMessage("Notation contains invalid characters. Only Rubik's Cube notation (R, U, F, B, L, D, ', 2, space) is allowed.")
                .MaximumLength(500)
                .WithMessage("Notation must not exceed 500 characters.");

            RuleFor(a => a.Description)
                .MaximumLength(1000)
                .WithMessage("Description must not exceed 1000 characters.");

            RuleFor(a => a.LessonId)
                .GreaterThan(0)
                .When(a => a.LessonId.HasValue)
                .WithMessage("LessonId, if provided, must be greater than 0.");
        }
    }
}
