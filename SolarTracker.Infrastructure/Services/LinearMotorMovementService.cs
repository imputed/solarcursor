using Microsoft.Extensions.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Interfaces.Hardware;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;
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
        Result<(LinearMotor LinearMotor, InstallationSite InstallationSite)> contextResult =
            await BuildContextAsync(linearMotorId, cancellationToken);
        if (!contextResult.IsSuccess)
        {
            ResultError error = contextResult.Error!.Value;
            return Result.NotFound(error.Code, error.Message);
        }

        await contextResult.Value.LinearMotor.MoveUpAsync(actuator.MoveUpAsync, durationMs, cancellationToken);
        return Result.Success();
    }

    public async ValueTask<Result> MoveDownAsync(
        int linearMotorId,
        int durationMs,
        CancellationToken cancellationToken)
    {
        Result<(LinearMotor LinearMotor, InstallationSite InstallationSite)> contextResult =
            await BuildContextAsync(linearMotorId, cancellationToken);
        if (!contextResult.IsSuccess)
        {
            ResultError error = contextResult.Error!.Value;
            return Result.NotFound(error.Code, error.Message);
        }

        await contextResult.Value.LinearMotor.MoveDownAsync(actuator.MoveDownAsync, durationMs, cancellationToken);
        return Result.Success();
    }

    private async ValueTask<Result<(LinearMotor LinearMotor, InstallationSite InstallationSite)>> BuildContextAsync(
        int linearMotorId,
        CancellationToken cancellationToken)
    {
        var linearMotor = await linearMotorQueryHandler.GetByIdAsync(linearMotorId, cancellationToken);
        if (linearMotor is null)
        {
            InfrastructureLog.LinearMotorNotFound(logger, linearMotorId);
            return Result<(LinearMotor LinearMotor, InstallationSite InstallationSite)>.NotFound(
                SolarTrackerErrorCatalog.LinearMotor.NotFound(linearMotorId));
        }

        var solarPanel = await solarPanelQueryHandler.GetByIdAsync(linearMotor.SolarPanelId, cancellationToken);
        if (solarPanel is null)
        {
            InfrastructureLog.SolarPanelNotFoundForLinearMotor(logger, linearMotor.SolarPanelId, linearMotorId);
            return Result<(LinearMotor LinearMotor, InstallationSite InstallationSite)>.NotFound(
                SolarTrackerErrorCatalog.SolarPanel.NotFound(linearMotor.SolarPanelId));
        }

        var installationSite = await installationSiteQueryHandler.GetByIdAsync(solarPanel.InstallationSiteId, cancellationToken);
        if (installationSite is null)
        {
            InfrastructureLog.InstallationSiteNotFoundForLinearMotor(
                logger,
                solarPanel.InstallationSiteId,
                linearMotorId);
            return Result<(LinearMotor LinearMotor, InstallationSite InstallationSite)>.NotFound(
                SolarTrackerErrorCatalog.InstallationSite.NotFound(solarPanel.InstallationSiteId));
        }

        return Result<(LinearMotor LinearMotor, InstallationSite InstallationSite)>.Success((linearMotor, installationSite));
    }
}
