using Innovative.Geometry;
using Innovative.SolarCalculator;
using Microsoft.Extensions.Logging;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Calculators;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Infrastructure.Calculators;

public sealed class SolarPanelCalculator(
    ISolarPanelQueryHandler solarPanelQueryHandler,
    IInstallationSiteQueryHandler installationSiteQueryHandler,
    ISolarTrackingConfigurationRepository configurationRepository,
    ILinearMotorMovementService linearMotorMovementService,
    ITiltMeasuringUnitPositionReader tiltMeasuringUnitPositionReader,
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
            LinearMotorMoveRequest request = new(context.Configuration.StepDurationMs);

            foreach (LinearMotor linearMotor in context.SolarPanel.LinearMotors.OrderBy(motor => motor.Id))
            {
                Result moveResult = moveUp
                    ? await linearMotorMovementService.MoveUpAsync(linearMotor.Id, request, cancellationToken)
                    : await linearMotorMovementService.MoveDownAsync(linearMotor.Id, request, cancellationToken);

                if (!moveResult.IsSuccess)
                {
                    ResultError error = moveResult.Error!.Value;
                    return moveResult.IsNotFound
                        ? Result<SolarPanelCurrentPositionDto>.NotFound(error.Code, error.Message)
                        : Result<SolarPanelCurrentPositionDto>.Failure(error.Code, error.Message);
                }
            }

            currentState = await CreateCurrentPositionDtoAsync(context, cancellationToken);
        }

        logger.LogWarning(
            "MoveToOptimum reached the configured maximum step count for solar panel {SolarPanelId}.",
            solarPanelId);

        return Result<SolarPanelCurrentPositionDto>.Failure(
            "solar-panel-threshold-not-met",
            $"Solar panel {solarPanelId} did not reach the configured threshold in the allowed number of steps.");
    }

    private async ValueTask<Result<SolarPanelCalculationContext>> BuildContextAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        SolarPanel? solarPanel = await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken);
        if (solarPanel is null)
            return Result<SolarPanelCalculationContext>.NotFound(
                "solar-panel-not-found",
                $"Solar panel {solarPanelId} was not found.");

        if (solarPanel.TiltMeasuringUnit is null)
            return Result<SolarPanelCalculationContext>.Failure(
                "tilt-measuring-unit-missing",
                $"Solar panel {solarPanelId} does not have a tilt measuring unit.");

        if (solarPanel.LinearMotors.Count == 0)
            return Result<SolarPanelCalculationContext>.Failure(
                "linear-motors-missing",
                $"Solar panel {solarPanelId} does not have any linear motors.");

        InstallationSite? installationSite =
            await installationSiteQueryHandler.GetByIdAsync(solarPanel.InstallationSiteId, cancellationToken);
        if (installationSite is null)
            return Result<SolarPanelCalculationContext>.NotFound(
                "installation-site-not-found",
                $"Installation site {solarPanel.InstallationSiteId} was not found.");

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

        double optimalPosition = CalculateOptimalPosition(
            context.InstallationSite.Latitude,
            context.InstallationSite.Longitude);

        return new SolarPanelCurrentPositionDto(
            context.SolarPanel.Id,
            optimalPosition,
            measurement.Degrees);
    }

    private double CalculateOptimalPosition(decimal latitude, decimal longitude)
    {
        DateTimeOffset now = timeProvider.GetUtcNow();
        SolarTimes solarTimes = new(now, new Angle(latitude), new Angle(longitude));
        return Math.Clamp((double)solarTimes.SolarZenith, 0d, 90d);
    }
}