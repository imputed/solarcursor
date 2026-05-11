using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Hardware;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Results;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class LinearMotorMovementService(
    ILinearMotorQueryHandler linearMotorQueryHandler,
    ISolarPanelQueryHandler solarPanelQueryHandler,
    IInstallationSiteQueryHandler installationSiteQueryHandler,
    ILinearMotorActuator actuator) : ILinearMotorMovementService
{
    public async ValueTask<Result> MoveUpAsync(
        int linearMotorId,
        LinearMotorMoveRequest request,
        CancellationToken cancellationToken)
    {
        Result<LinearMotorMovementContext> contextResult =
            await BuildContextAsync(linearMotorId, request, cancellationToken);
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
        LinearMotorMoveRequest request,
        CancellationToken cancellationToken)
    {
        Result<LinearMotorMovementContext> contextResult =
            await BuildContextAsync(linearMotorId, request, cancellationToken);
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
        LinearMotorMoveRequest request,
        CancellationToken cancellationToken)
    {
        var linearMotor = await linearMotorQueryHandler.GetByIdAsync(linearMotorId, cancellationToken);
        if (linearMotor is null)
            return Result<LinearMotorMovementContext>.NotFound(
                "linear-motor-not-found",
                $"Linear motor {linearMotorId} was not found.");

        var solarPanel = await solarPanelQueryHandler.GetByIdAsync(linearMotor.SolarPanelId, cancellationToken);
        if (solarPanel is null)
            return Result<LinearMotorMovementContext>.NotFound(
                "solar-panel-not-found",
                $"Solar panel {linearMotor.SolarPanelId} was not found.");

        var installationSite = await installationSiteQueryHandler.GetByIdAsync(solarPanel.InstallationSiteId, cancellationToken);
        if (installationSite is null)
            return Result<LinearMotorMovementContext>.NotFound(
                "installation-site-not-found",
                $"Installation site {solarPanel.InstallationSiteId} was not found.");

        return Result<LinearMotorMovementContext>.Success(
            new LinearMotorMovementContext(
                linearMotor.Id,
                installationSite.Id,
                installationSite.Latitude,
                installationSite.Longitude,
                linearMotor.MoveUpGpioPin,
                linearMotor.MoveDownGpioPin,
                request.DurationMs));
    }
}
