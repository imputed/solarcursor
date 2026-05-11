using SolarTracker.Application.Dtos;
using SolarTracker.Application.Results;

namespace SolarTracker.Application.Interfaces.Calculators;

public interface ISolarmoduleCalculator
{
    ValueTask<Result<SolarPanelCurrentPositionDto>> GetCurrentPositionAsync(
        int solarPanelId,
        CancellationToken cancellationToken);

    ValueTask<Result<SolarPanelCurrentPositionDto>> MoveToOptimumAsync(
        int solarPanelId,
        CancellationToken cancellationToken);
}
