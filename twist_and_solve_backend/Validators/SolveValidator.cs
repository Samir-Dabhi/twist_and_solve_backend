using FluentValidation;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Validators
{
    public class SolveValidator : AbstractValidator<SolveModel>
    {
        public SolveValidator()
        {
            RuleFor(s => s.SolveId)
                .GreaterThan(0)
                .WithMessage("SolveId must be greater than 0.");

            RuleFor(s => s.UserId)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0.");

            RuleFor(s => s.SolveTime)
                .GreaterThan(0)
                .WithMessage("SolveTime must be greater than 0 seconds.");

            RuleFor(s => s.SolveDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("SolveDate cannot be in the future.");

            RuleFor(s => s.Method)
                .NotEmpty()
                .WithMessage("Method is required.")
                .MaximumLength(50)
                .WithMessage("Method must not exceed 50 characters.");

            RuleFor(s => s.MovesCount)
                .GreaterThan(0)
                .When(s => s.MovesCount.HasValue)
                .WithMessage("MovesCount, if provided, must be greater than 0.");

            RuleFor(s => s.SolveResult)
                .NotEmpty()
                .WithMessage("SolveResult is required.")
                .Must(result => result == "Success" || result == "Failed")
                .WithMessage("SolveResult must be either 'Success' or 'Failed'.");

            RuleFor(s => s.Scramble)
                .NotEmpty()
                .WithMessage("Scramble is required.")
                .Matches("^[RUFBLD'2 ]+$")
                .WithMessage("Scramble contains invalid characters. Only Rubik's Cube notation (R, U, F, B, L, D, ', 2, space) is allowed.")
                .MaximumLength(500)
                .WithMessage("Scramble must not exceed 500 characters.");
        }
    }
}
