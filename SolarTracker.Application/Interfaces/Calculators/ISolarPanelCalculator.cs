using SolarTracker.Application.Dtos;
using SolarTracker.Application.Results;

namespace SolarTracker.Application.Interfaces.Calculators;

public interface ISolarPanelCalculator
{
    ValueTask<Result<SolarPanelCurrentPositionDto>> MoveToOptimumAsync(int solarPanelId, CancellationToken cancellationToken);
}
