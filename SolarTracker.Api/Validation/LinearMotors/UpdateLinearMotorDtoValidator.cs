using FluentValidation;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Validation.LinearMotors;

public sealed class UpdateLinearMotorDtoValidator : AbstractValidator<UpdateLinearMotorDto>
{
    public UpdateLinearMotorDtoValidator()
    {
        RuleFor(d => d.Id).GreaterThan(0);
        RuleFor(d => d.InstallationSiteId).GreaterThan(0);
        RuleFor(d => d.Name).MaximumLength(128);
    }
}
