namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class SolarTrackingConfigurationDb
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public SolarPanelDb SolarPanel { get; set; } = null!;

    public double PositionThresholdDegrees { get; set; }

    public int StepDurationMs { get; set; }

    public int MaxAdjustmentSteps { get; set; }
}
