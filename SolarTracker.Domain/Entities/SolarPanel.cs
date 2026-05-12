using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Errors;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Domain.Entities;

public sealed class SolarPanel
{
    public int Id { get; set; }

    public int InstallationSiteId { get; set; }

    public string? SerialNumber { get; set; }

    public SolarTrackingConfiguration? SolarTrackingConfiguration { get; set; }

    public TiltMeasuringUnit? TiltMeasuringUnit { get; set; }

    public CurrentMeasuringUnit? CurrentMeasuringUnit { get; set; }

    public ICollection<LinearMotor> LinearMotors { get; set; } = [];

    public async ValueTask<TiltMeasurement> GetPosition(ITiltMeasuringUnitPositionReader positionReader, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(positionReader);
        
        if (TiltMeasuringUnit is null)
            throw new InvalidOperationException(DomainTextCatalog.SolarPanel.PositionRequiresTiltMeasuringUnit());

        return await TiltMeasuringUnit.GetCurrentPosition(positionReader, cancellationToken);
    }

    public SolarPanelMovementValidationResult ValidateSolarPanelForMovement()
    {
        if (TiltMeasuringUnit is null)
            return SolarPanelMovementValidationResult.TiltMeasuringUnitMissing;

        if (LinearMotors.Count == 0)
            return SolarPanelMovementValidationResult.LinearMotorsMissing;

        return SolarPanelMovementValidationResult.Valid;
    }

    public async ValueTask<SolarPanelMoveResult> MoveToTargetPositionAsync(double targetPosition, SolarTrackingConfiguration configuration, ITiltMeasuringUnitPositionReader positionReader, ISteeringCommandReceiver steeringCommandReceiver, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(positionReader);
        ArgumentNullException.ThrowIfNull(steeringCommandReceiver);

        SolarPanelMovementValidationResult validationResult = ValidateSolarPanelForMovement();
        switch (validationResult)
        {
            case SolarPanelMovementValidationResult.Valid:
                break;
            case SolarPanelMovementValidationResult.TiltMeasuringUnitMissing:
                throw new InvalidOperationException(DomainTextCatalog.SolarPanel.PositionRequiresTiltMeasuringUnit());
            case SolarPanelMovementValidationResult.LinearMotorsMissing:
                throw new InvalidOperationException(DomainTextCatalog.SolarPanel.MovementRequiresLinearMotors());
            default:
                throw new InvalidOperationException(
                    DomainTextCatalog.SolarPanel.UnsupportedMovementValidationResult(validationResult));
        }

        TiltMeasurement currentMeasurement = await GetPosition(positionReader, cancellationToken);
        for (int step = 0; step < configuration.MaxAdjustmentSteps; step++)
        {
            double delta = targetPosition - currentMeasurement.Degrees;
            if (Math.Abs(delta) <= configuration.PositionThresholdDegrees)
                return SolarPanelMoveResult.Success(currentMeasurement);

            bool moveUp = delta > 0d;
            List<LinearMotor> movedMotors = [];
            foreach (LinearMotor linearMotor in LinearMotors.OrderBy(motor => motor.Id))
            {
                string? failureMessage = await ExecuteMovementAsync(linearMotor, moveUp, configuration.StepDurationMs, steeringCommandReceiver, cancellationToken);
                if (failureMessage is null)
                {
                    movedMotors.Add(linearMotor);
                    continue;
                }

                return await HandleMovementFailureAsync(movedMotors, moveUp, configuration.StepDurationMs, linearMotor.Id, failureMessage, steeringCommandReceiver, cancellationToken);
            }

            currentMeasurement = await GetPosition(positionReader, cancellationToken);
        }

        return SolarPanelMoveResult.ThresholdNotMet(currentMeasurement);
    }

    private static async ValueTask<SolarPanelMoveResult> HandleMovementFailureAsync(IReadOnlyList<LinearMotor> movedMotors, bool moveUp, int durationMs, int failedLinearMotorId, string failureMessage, ISteeringCommandReceiver steeringCommandReceiver, CancellationToken cancellationToken)
    {
        if (movedMotors.Count == 0)
            return SolarPanelMoveResult.MovementFailed(failedLinearMotorId, failureMessage);

        string? recoveryFailureMessage = await RecoverStepAsync(movedMotors, moveUp, durationMs, steeringCommandReceiver, cancellationToken);
        if (recoveryFailureMessage is null)
            return SolarPanelMoveResult.MovementStepReverted(failedLinearMotorId, failureMessage);

        return SolarPanelMoveResult.MovementRecoveryFailed(failedLinearMotorId, failureMessage, recoveryFailureMessage);
    }

    private static async ValueTask<string?> RecoverStepAsync(IReadOnlyList<LinearMotor> movedMotors, bool moveUp, int durationMs, ISteeringCommandReceiver steeringCommandReceiver, CancellationToken cancellationToken)
    {
        for (int index = movedMotors.Count - 1; index >= 0; index--)
        {
            LinearMotor linearMotor = movedMotors[index];
            string? recoveryFailureMessage = await ExecuteMovementAsync(linearMotor, !moveUp, durationMs, steeringCommandReceiver, cancellationToken);
            if (recoveryFailureMessage is not null)
                return recoveryFailureMessage;
        }

        return null;
    }

    private static async ValueTask<string?> ExecuteMovementAsync(LinearMotor linearMotor, bool moveUp, int durationMs, ISteeringCommandReceiver steeringCommandReceiver, CancellationToken cancellationToken)
    {
        try
        {
            if (moveUp)
                await linearMotor.MoveUpAsync(steeringCommandReceiver, cancellationToken);
            else
                await linearMotor.MoveDownAsync(steeringCommandReceiver, cancellationToken);

            try
            {
                await Task.Delay(durationMs, cancellationToken);
            }
            finally
            {
                await linearMotor.StopAsync(steeringCommandReceiver, CancellationToken.None);
            }

            return null;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            return exception.Message;
        }
    }
}
