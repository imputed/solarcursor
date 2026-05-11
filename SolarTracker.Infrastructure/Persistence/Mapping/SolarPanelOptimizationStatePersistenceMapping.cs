using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class SolarPanelOptimizationStatePersistenceMapping
{
    public static SolarPanelOptimizationState ToDomain(SolarPanelOptimizationStateDb db) =>
        new()
        {
            Id = db.Id,
            SolarPanelId = db.SolarPanelId,
            IsEnabled = db.IsEnabled,
        };

    public static SolarPanelOptimizationStateDb ToDb(SolarPanelOptimizationState domain) =>
        new()
        {
            Id = domain.Id,
            SolarPanelId = domain.SolarPanelId,
            IsEnabled = domain.IsEnabled,
        };

    public static void CopyScalars(SolarPanelOptimizationStateDb target, SolarPanelOptimizationState source)
    {
        target.SolarPanelId = source.SolarPanelId;
        target.IsEnabled = source.IsEnabled;
    }
}
