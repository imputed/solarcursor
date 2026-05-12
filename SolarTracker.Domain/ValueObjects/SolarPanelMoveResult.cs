namespace SolarTracker.Domain.ValueObjects;

public readonly record struct SolarPanelMoveResult
{
    public SolarPanelMoveResultStatus Status { get; init; }

    public TiltMeasurement? Measurement { get; init; }

    public int? FailedLinearMotorId { get; init; }

    public string? FailureMessage { get; init; }

    public string? RecoveryFailureMessage { get; init; }

    public bool IsSuccess => Status == SolarPanelMoveResultStatus.Success;

    public static SolarPanelMoveResult Success(TiltMeasurement measurement) =>
        new()
        {
            Status = SolarPanelMoveResultStatus.Success,
            Measurement = measurement,
        };

    public static SolarPanelMoveResult ThresholdNotMet(TiltMeasurement measurement) =>
        new()
        {
            Status = SolarPanelMoveResultStatus.ThresholdNotMet,
            Measurement = measurement,
        };

    public static SolarPanelMoveResult MovementFailed(int failedLinearMotorId, string failureMessage) =>
        new()
        {
            Status = SolarPanelMoveResultStatus.MovementFailed,
            FailedLinearMotorId = failedLinearMotorId,
            FailureMessage = failureMessage,
        };

    public static SolarPanelMoveResult MovementStepReverted(int failedLinearMotorId, string failureMessage) =>
        new()
        {
            Status = SolarPanelMoveResultStatus.MovementStepReverted,
            FailedLinearMotorId = failedLinearMotorId,
            FailureMessage = failureMessage,
        };

    public static SolarPanelMoveResult MovementRecoveryFailed(
        int failedLinearMotorId,
        string failureMessage,
        string recoveryFailureMessage) =>
        new()
        {
            Status = SolarPanelMoveResultStatus.MovementRecoveryFailed,
            FailedLinearMotorId = failedLinearMotorId,
            FailureMessage = failureMessage,
            RecoveryFailureMessage = recoveryFailureMessage,
        };
}
