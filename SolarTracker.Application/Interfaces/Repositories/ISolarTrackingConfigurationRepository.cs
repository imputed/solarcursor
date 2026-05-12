using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface ISolarTrackingConfigurationRepository
{
    ValueTask<SolarTrackingConfiguration> GetBySolarPanelIdAsync(int solarPanelId, CancellationToken cancellationToken);

    ValueTask<SolarTrackingConfiguration> UpsertAsync(SolarTrackingConfiguration entity, CancellationToken cancellationToken);
}
