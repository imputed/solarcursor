namespace SolarTracker.Domain.Entities;

public sealed class SolarTrackingConfiguration
{
    public const double DefaultPositionThresholdDegrees = 1d;
    public const int DefaultStepDurationMs = 500;
    public const int DefaultMaxAdjustmentSteps = 20;

    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public double PositionThresholdDegrees { get; set; } = DefaultPositionThresholdDegrees;

    public int StepDurationMs { get; set; } = DefaultStepDurationMs;

    public int MaxAdjustmentSteps { get; set; } = DefaultMaxAdjustmentSteps;

    public static SolarTrackingConfiguration CreateDefault(int solarPanelId) =>
        new() { SolarPanelId = solarPanelId };
}
