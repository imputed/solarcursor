using Microsoft.Extensions.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Interfaces.Hardware;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Results;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Services;

public sealed class LinearMotorMovementService(
    ILinearMotorQueryHandler linearMotorQueryHandler,
    ISolarPanelQueryHandler solarPanelQueryHandler,
    IInstallationSiteQueryHandler installationSiteQueryHandler,
    ILinearMotorActuator actuator,
    ILogger<LinearMotorMovementService> logger)
{
    public async ValueTask<Result> MoveUpAsync(
        int linearMotorId,
        int durationMs,
        CancellationToken cancellationToken)
    {
        Result<LinearMotorMovementContext> contextResult =
            await BuildContextAsync(linearMotorId, durationMs, cancellationToken);
        if (!contextResult.IsSuccess)
        {
            ResultError error = contextResult.Error!.Value;
            return Result.NotFound(error.Code, error.Message);
        }

        await actuator.MoveUpAsync(contextResult.Value, cancellationToken);
        return Result.Success();
    }

    public async ValueTask<Result> MoveDownAsync(
        int linearMotorId,
        int durationMs,
        CancellationToken cancellationToken)
    {
        Result<LinearMotorMovementContext> contextResult =
            await BuildContextAsync(linearMotorId, durationMs, cancellationToken);
        if (!contextResult.IsSuccess)
        {
            ResultError error = contextResult.Error!.Value;
            return Result.NotFound(error.Code, error.Message);
        }

        await actuator.MoveDownAsync(contextResult.Value, cancellationToken);
        return Result.Success();
    }

    private async ValueTask<Result<LinearMotorMovementContext>> BuildContextAsync(
        int linearMotorId,
        int durationMs,
        CancellationToken cancellationToken)
    {
        var linearMotor = await linearMotorQueryHandler.GetByIdAsync(linearMotorId, cancellationToken);
        if (linearMotor is null)
        {
            InfrastructureLog.LinearMotorNotFound(logger, linearMotorId);
            return Result<LinearMotorMovementContext>.NotFound(SolarTrackerErrorCatalog.LinearMotor.NotFound(linearMotorId));
        }

        var solarPanel = await solarPanelQueryHandler.GetByIdAsync(linearMotor.SolarPanelId, cancellationToken);
        if (solarPanel is null)
        {
            InfrastructureLog.SolarPanelNotFoundForLinearMotor(logger, linearMotor.SolarPanelId, linearMotorId);
            return Result<LinearMotorMovementContext>.NotFound(
                SolarTrackerErrorCatalog.SolarPanel.NotFound(linearMotor.SolarPanelId));
        }

        var installationSite = await installationSiteQueryHandler.GetByIdAsync(solarPanel.InstallationSiteId, cancellationToken);
        if (installationSite is null)
        {
            InfrastructureLog.InstallationSiteNotFoundForLinearMotor(
                logger,
                solarPanel.InstallationSiteId,
                linearMotorId);
            return Result<LinearMotorMovementContext>.NotFound(
                SolarTrackerErrorCatalog.InstallationSite.NotFound(solarPanel.InstallationSiteId));
        }

        return Result<LinearMotorMovementContext>.Success(
            new LinearMotorMovementContext(
                linearMotor.Id,
                installationSite.Id,
                installationSite.Latitude,
                installationSite.Longitude,
                linearMotor.MoveUpGpioPin,
                linearMotor.MoveDownGpioPin,
                durationMs));
    }
}
