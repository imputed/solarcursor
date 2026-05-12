using SolarTracker.Application.Dtos;
using SolarTracker.Application.Results;

namespace SolarTracker.Application.Interfaces.Services;

public interface ISolarPanelOptimizationStateService
{
    ValueTask<Result<SolarPanelOptimizationStateDto>> GetAsync(int solarPanelId, CancellationToken cancellationToken);

    ValueTask<Result<SolarPanelOptimizationStateDto>> UpdateAsync(int solarPanelId, UpdateSolarPanelOptimizationStateDto dto, CancellationToken cancellationToken);
}
