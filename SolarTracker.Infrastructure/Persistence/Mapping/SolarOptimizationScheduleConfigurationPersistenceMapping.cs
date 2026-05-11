using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class SolarOptimizationScheduleConfigurationPersistenceMapping
{
    public static SolarOptimizationScheduleConfiguration ToDomain(SolarOptimizationScheduleConfigurationDb db) =>
        new()
        {
            Id = db.Id,
            IntervalMinutes = db.IntervalMinutes,
        };

    public static SolarOptimizationScheduleConfigurationDb ToDb(SolarOptimizationScheduleConfiguration domain) =>
        new()
        {
            Id = domain.Id,
            IntervalMinutes = domain.IntervalMinutes,
        };

    public static void CopyScalars(
        SolarOptimizationScheduleConfigurationDb target,
        SolarOptimizationScheduleConfiguration source) =>
        target.IntervalMinutes = source.IntervalMinutes;
}
