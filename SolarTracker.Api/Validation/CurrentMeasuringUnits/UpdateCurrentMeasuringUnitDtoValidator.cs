using FluentValidation;
using SolarTracker.Application.Dtos;

namespace SolarTracker.Api.Validation.CurrentMeasuringUnits;

public sealed class UpdateCurrentMeasuringUnitDtoValidator : AbstractValidator<UpdateCurrentMeasuringUnitDto>
{
    public UpdateCurrentMeasuringUnitDtoValidator()
    {
        RuleFor(d => d.Id).GreaterThan(0);
        RuleFor(d => d.SolarPanelId).GreaterThan(0);
        RuleFor(d => d.Name).MaximumLength(128);
        RuleFor(d => d.GpioPin).InclusiveBetween(0, 27);
    }
}
