using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface ISolarPanelOptimizationStateRepository
{
    ValueTask<SolarPanelOptimizationState> GetBySolarPanelIdAsync(int solarPanelId, CancellationToken cancellationToken);

    ValueTask<SolarPanelOptimizationState> UpsertAsync(SolarPanelOptimizationState entity, CancellationToken cancellationToken);

    ValueTask<IReadOnlyList<int>> ListEnabledSolarPanelIdsAsync(CancellationToken cancellationToken);
}
