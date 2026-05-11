using SolarTracker.Application.Dtos;

namespace SolarTracker.Application.Interfaces.Services;

public interface ISolarOptimizationScheduleConfigurationService
{
    ValueTask<SolarOptimizationScheduleConfigurationDto> GetAsync(CancellationToken cancellationToken);

    ValueTask<SolarOptimizationScheduleConfigurationDto> UpdateAsync(
        UpdateSolarOptimizationScheduleConfigurationDto dto,
        CancellationToken cancellationToken);
}
