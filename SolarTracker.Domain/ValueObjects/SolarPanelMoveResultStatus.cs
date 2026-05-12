namespace SolarTracker.Domain.ValueObjects;

public enum SolarPanelMoveResultStatus
{
    Success,
    ThresholdNotMet,
    MovementFailed,
    MovementStepReverted,
    MovementRecoveryFailed,
}
