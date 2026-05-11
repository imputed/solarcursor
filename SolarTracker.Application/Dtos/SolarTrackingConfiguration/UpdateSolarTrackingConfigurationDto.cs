namespace SolarTracker.Application.Dtos;

public sealed record UpdateSolarTrackingConfigurationDto(
    double PositionThresholdDegrees,
    int StepDurationMs,
    int MaxAdjustmentSteps);
