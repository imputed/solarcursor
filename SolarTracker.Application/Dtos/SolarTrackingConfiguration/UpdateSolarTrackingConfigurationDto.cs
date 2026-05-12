namespace SolarTracker.Application.Dtos.SolarTrackingConfiguration;

public sealed record UpdateSolarTrackingConfigurationDto(
    double PositionThresholdDegrees,
    int StepDurationMs,
    int MaxAdjustmentSteps);
