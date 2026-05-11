namespace SolarTracker.Application.Dtos;

public sealed record SolarTrackingConfigurationDto(
    int SolarPanelId,
    double PositionThresholdDegrees,
    int StepDurationMs,
    int MaxAdjustmentSteps);
