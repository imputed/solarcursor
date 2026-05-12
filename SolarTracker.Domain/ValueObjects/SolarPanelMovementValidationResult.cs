namespace SolarTracker.Domain.ValueObjects;

public enum SolarPanelMovementValidationResult
{
    Valid,
    TiltMeasuringUnitMissing,
    LinearMotorsMissing,
}
