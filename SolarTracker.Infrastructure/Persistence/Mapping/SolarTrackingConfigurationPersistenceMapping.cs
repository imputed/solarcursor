using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class SolarTrackingConfigurationPersistenceMapping
{
    public static SolarTrackingConfiguration ToDomain(SolarTrackingConfigurationDb db) =>
        new()
        {
            Id = db.Id,
            SolarPanelId = db.SolarPanelId,
            PositionThresholdDegrees = db.PositionThresholdDegrees,
            StepDurationMs = db.StepDurationMs,
            MaxAdjustmentSteps = db.MaxAdjustmentSteps,
        };

    public static SolarTrackingConfigurationDb ToDb(SolarTrackingConfiguration domain) =>
        new()
        {
            Id = domain.Id,
            SolarPanelId = domain.SolarPanelId,
            PositionThresholdDegrees = domain.PositionThresholdDegrees,
            StepDurationMs = domain.StepDurationMs,
            MaxAdjustmentSteps = domain.MaxAdjustmentSteps,
        };

    public static void CopyScalars(SolarTrackingConfigurationDb target, SolarTrackingConfiguration source)
    {
        target.SolarPanelId = source.SolarPanelId;
        target.PositionThresholdDegrees = source.PositionThresholdDegrees;
        target.StepDurationMs = source.StepDurationMs;
        target.MaxAdjustmentSteps = source.MaxAdjustmentSteps;
    }
}
