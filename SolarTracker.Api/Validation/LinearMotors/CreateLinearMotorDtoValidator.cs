using FluentValidation;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Validation.LinearMotors;

public sealed class CreateLinearMotorDtoValidator : AbstractValidator<CreateLinearMotorDto>
{
    public CreateLinearMotorDtoValidator()
    {
        RuleFor(d => d.InstallationSiteId).GreaterThan(0);
        RuleFor(d => d.Name).MaximumLength(128);
    }
}
