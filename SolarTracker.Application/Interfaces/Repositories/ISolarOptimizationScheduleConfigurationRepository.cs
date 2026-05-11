using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface ISolarOptimizationScheduleConfigurationRepository
{
    ValueTask<SolarOptimizationScheduleConfiguration> GetAsync(CancellationToken cancellationToken);

    ValueTask<SolarOptimizationScheduleConfiguration> UpsertAsync(
        SolarOptimizationScheduleConfiguration entity,
        CancellationToken cancellationToken);
}
