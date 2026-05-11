using FluentValidation;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Validation.LinearMotors;

public sealed class CreateLinearMotorDtoValidator : AbstractValidator<CreateLinearMotorDto>
{
    public CreateLinearMotorDtoValidator()
    {
        RuleFor(d => d.SolarPanelId).GreaterThan(0);
        RuleFor(d => d.Name).MaximumLength(128);
        RuleFor(d => d.MoveUpGpioPin).InclusiveBetween(0, 27);
        RuleFor(d => d.MoveDownGpioPin).InclusiveBetween(0, 27);
        RuleFor(d => d).Must(d => d.MoveUpGpioPin != d.MoveDownGpioPin)
            .WithMessage("MoveUpGpioPin and MoveDownGpioPin must differ.");
    }
}
