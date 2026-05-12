using FluentValidation;
using SolarTracker.Application.Dtos.TiltMeasuringUnit;

namespace SolarTracker.Api.Validation.TiltMeasuringUnits;

public sealed class CreateTiltMeasuringUnitDtoValidator : AbstractValidator<CreateTiltMeasuringUnitDto>
{
    public CreateTiltMeasuringUnitDtoValidator()
    {
        RuleFor(d => d.SolarPanelId).GreaterThan(0);
        RuleFor(d => d.Name).MaximumLength(128);
        RuleFor(d => d.GpioPin).InclusiveBetween(0, 27);
    }
}
