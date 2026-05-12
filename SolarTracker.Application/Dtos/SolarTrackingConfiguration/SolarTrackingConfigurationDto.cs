namespace SolarTracker.Application.Dtos.SolarTrackingConfiguration;

public sealed record SolarTrackingConfigurationDto(
    int SolarPanelId,
    double PositionThresholdDegrees,
    int StepDurationMs,
    int MaxAdjustmentSteps);
