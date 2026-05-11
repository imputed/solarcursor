using FluentValidation;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Validation.LinearMotors;

public sealed class LinearMotorMoveRequestValidator : AbstractValidator<LinearMotorMoveRequest>
{
    public LinearMotorMoveRequestValidator()
    {
        RuleFor(d => d.DurationMs).InclusiveBetween(50, 10_000);
    }
}
