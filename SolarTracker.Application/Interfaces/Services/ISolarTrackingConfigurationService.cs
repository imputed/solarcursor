using SolarTracker.Application.Dtos;
using SolarTracker.Application.Results;

namespace SolarTracker.Application.Interfaces.Services;

public interface ISolarTrackingConfigurationService
{
    ValueTask<Result<SolarTrackingConfigurationDto>> GetAsync(
        int solarPanelId,
        CancellationToken cancellationToken);

    ValueTask<Result<SolarTrackingConfigurationDto>> UpdateAsync(
        int solarPanelId,
        UpdateSolarTrackingConfigurationDto dto,
        CancellationToken cancellationToken);
}
