using FluentValidation;
using SolarTracker.Application.Dtos.SolarOptimizationScheduleConfiguration;

namespace SolarTracker.Api.Validation.SolarOptimizationScheduleConfiguration;

public sealed class UpdateSolarOptimizationScheduleConfigurationDtoValidator
    : AbstractValidator<UpdateSolarOptimizationScheduleConfigurationDto>
{
    public UpdateSolarOptimizationScheduleConfigurationDtoValidator()
    {
        RuleFor(d => d.IntervalMinutes).InclusiveBetween(1, 1_440);
    }
}
