using FluentValidation;
using SolarTracker.Application.Dtos.SolarTrackingConfiguration;

namespace SolarTracker.Api.Validation.SolarTrackingConfiguration;

public sealed class UpdateSolarTrackingConfigurationDtoValidator
    : AbstractValidator<UpdateSolarTrackingConfigurationDto>
{
    public UpdateSolarTrackingConfigurationDtoValidator()
    {
        RuleFor(d => d.PositionThresholdDegrees).InclusiveBetween(0.1d, 45d);
        RuleFor(d => d.StepDurationMs).InclusiveBetween(50, 10_000);
        RuleFor(d => d.MaxAdjustmentSteps).InclusiveBetween(1, 500);
    }
}
