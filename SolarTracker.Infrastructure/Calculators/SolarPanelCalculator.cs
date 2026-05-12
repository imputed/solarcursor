using Microsoft.Extensions.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Calculators;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;
using SolarTracker.Infrastructure.Logging;
using SolarTracker.Infrastructure.Services;

namespace SolarTracker.Infrastructure.Calculators;

public sealed class SolarPanelCalculator(
    ISolarPanelQueryHandler solarPanelQueryHandler,
    IInstallationSiteQueryHandler installationSiteQueryHandler,
    ISolarTrackingConfigurationRepository configurationRepository,
    LinearMotorMovementService linearMotorMovementService,
    ITiltMeasuringUnitPositionReader tiltMeasuringUnitPositionReader,
    ISolarOptimalPositionCalculator solarOptimalPositionCalculator,
    TimeProvider timeProvider,
    ILogger<SolarPanelCalculator> logger) : ISolarPanelCalculator
{
    public async ValueTask<Result<SolarPanelCurrentPositionDto>> GetCurrentPositionAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        Result<SolarPanelCalculationContext> contextResult = await BuildContextAsync(solarPanelId, cancellationToken);
        if (!contextResult.IsSuccess)
        {
            ResultError error = contextResult.Error!.Value;
            return Result<SolarPanelCurrentPositionDto>.NotFound(error.Code, error.Message);
        }

        return Result<SolarPanelCurrentPositionDto>.Success(
            await CreateCurrentPositionDtoAsync(contextResult.Value, cancellationToken));
    }

    public async ValueTask<Result<SolarPanelCurrentPositionDto>> MoveToOptimumAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        Result<SolarPanelCalculationContext> contextResult = await BuildContextAsync(solarPanelId, cancellationToken);
        if (!contextResult.IsSuccess)
        {
            ResultError error = contextResult.Error!.Value;
            return Result<SolarPanelCurrentPositionDto>.NotFound(error.Code, error.Message);
        }

        SolarPanelCalculationContext context = contextResult.Value;
        SolarPanelCurrentPositionDto currentState = await CreateCurrentPositionDtoAsync(context, cancellationToken);
        for (int step = 0; step < context.Configuration.MaxAdjustmentSteps; step++)
        {
            double delta = currentState.OptimalPosition - currentState.CurrentPosition;
            if (Math.Abs(delta) <= context.Configuration.PositionThresholdDegrees)
                return Result<SolarPanelCurrentPositionDto>.Success(currentState);

            bool moveUp = delta > 0d;
            List<int> movedMotorIds = [];

            foreach (LinearMotor linearMotor in context.SolarPanel.LinearMotors.OrderBy(motor => motor.Id))
            {
                Result moveResult = moveUp
                    ? await linearMotorMovementService.MoveUpAsync(
                        linearMotor.Id,
                        context.Configuration.StepDurationMs,
                        cancellationToken)
                    : await linearMotorMovementService.MoveDownAsync(
                        linearMotor.Id,
                        context.Configuration.StepDurationMs,
                        cancellationToken);

                if (!moveResult.IsSuccess)
                    return await HandleMovementFailureAsync(
                        solarPanelId,
                        movedMotorIds,
                        moveUp,
                        context.Configuration.StepDurationMs,
                        linearMotor.Id,
                        moveResult,
                        cancellationToken);

                movedMotorIds.Add(linearMotor.Id);
            }

            currentState = await CreateCurrentPositionDtoAsync(context, cancellationToken);
        }

        InfrastructureLog.MoveToOptimumMaxStepsReached(logger, solarPanelId);

        return Result<SolarPanelCurrentPositionDto>.Failure(
            SolarTrackerErrorCatalog.SolarPanel.ThresholdNotMet(solarPanelId));
    }

    private async ValueTask<Result<SolarPanelCalculationContext>> BuildContextAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        SolarPanel? solarPanel = await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken);
        if (solarPanel is null)
            return Result<SolarPanelCalculationContext>.NotFound(SolarTrackerErrorCatalog.SolarPanel.NotFound(solarPanelId));

        if (solarPanel.TiltMeasuringUnit is null)
            return Result<SolarPanelCalculationContext>.Failure(
                SolarTrackerErrorCatalog.SolarPanel.TiltMeasuringUnitMissing(solarPanelId));

        if (solarPanel.LinearMotors.Count == 0)
            return Result<SolarPanelCalculationContext>.Failure(
                SolarTrackerErrorCatalog.SolarPanel.LinearMotorsMissing(solarPanelId));

        InstallationSite? installationSite =
            await installationSiteQueryHandler.GetByIdAsync(solarPanel.InstallationSiteId, cancellationToken);
        if (installationSite is null)
            return Result<SolarPanelCalculationContext>.NotFound(
                SolarTrackerErrorCatalog.InstallationSite.NotFound(solarPanel.InstallationSiteId));

        SolarTrackingConfiguration configuration =
            await configurationRepository.GetBySolarPanelIdAsync(solarPanelId, cancellationToken);
        return Result<SolarPanelCalculationContext>.Success(
            new SolarPanelCalculationContext(solarPanel, installationSite, configuration));
    }

    private async ValueTask<SolarPanelCurrentPositionDto> CreateCurrentPositionDtoAsync(
        SolarPanelCalculationContext context,
        CancellationToken cancellationToken)
    {
        TiltMeasurement measurement = await context.SolarPanel.TiltMeasuringUnit!
            .GetCurrentPosition(tiltMeasuringUnitPositionReader, cancellationToken);

        double optimalPosition = context.InstallationSite.GetOptimalPosition(
            solarOptimalPositionCalculator,
            timeProvider.GetUtcNow());

        return new SolarPanelCurrentPositionDto(
            context.SolarPanel.Id,
            optimalPosition,
            measurement.Degrees);
    }

    private async ValueTask<Result<SolarPanelCurrentPositionDto>> HandleMovementFailureAsync(
        int solarPanelId,
        IReadOnlyList<int> movedMotorIds,
        bool moveUp,
        int durationMs,
        int failedLinearMotorId,
        Result moveResult,
        CancellationToken cancellationToken)
    {
        ResultError moveError = moveResult.Error!.Value;
        if (movedMotorIds.Count == 0)
            return moveResult.IsNotFound
                ? Result<SolarPanelCurrentPositionDto>.NotFound(moveError.Code, moveError.Message)
                : Result<SolarPanelCurrentPositionDto>.Failure(moveError.Code, moveError.Message);

        InfrastructureLog.RecoveringSolarPanelMovement(
            logger,
            solarPanelId,
            failedLinearMotorId,
            moveError.Code,
            moveError.Message);

        Result recoveryResult = await RecoverStepAsync(movedMotorIds, moveUp, durationMs, cancellationToken);
        if (!recoveryResult.IsSuccess)
        {
            ResultError recoveryError = recoveryResult.Error!.Value;
            return Result<SolarPanelCurrentPositionDto>.Failure(
                SolarTrackerErrorCatalog.SolarPanel.MovementRecoveryFailed(
                    solarPanelId,
                    failedLinearMotorId,
                    moveError,
                    recoveryError));
        }

        return Result<SolarPanelCurrentPositionDto>.Failure(
            SolarTrackerErrorCatalog.SolarPanel.MovementStepReverted(solarPanelId, failedLinearMotorId, moveError));
    }

    private async ValueTask<Result> RecoverStepAsync(
        IReadOnlyList<int> movedMotorIds,
        bool moveUp,
        int durationMs,
        CancellationToken cancellationToken)
    {
        for (int index = movedMotorIds.Count - 1; index >= 0; index--)
        {
            int motorId = movedMotorIds[index];
            Result recoveryResult = moveUp
                ? await linearMotorMovementService.MoveDownAsync(motorId, durationMs, cancellationToken)
                : await linearMotorMovementService.MoveUpAsync(motorId, durationMs, cancellationToken);

            if (!recoveryResult.IsSuccess)
            {
                ResultError error = recoveryResult.Error!.Value;
                return Result.Failure(SolarTrackerErrorCatalog.LinearMotor.RecoveryFailed(motorId, error));
            }
        }

        return Result.Success();
    }
}