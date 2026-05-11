using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Hardware;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Services;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class LinearMotorMovementService(
    ILinearMotorQueryHandler linearMotorQueryHandler,
    ISolarPanelQueryHandler solarPanelQueryHandler,
    IInstallationSiteQueryHandler installationSiteQueryHandler,
    ILinearMotorActuator actuator) : ILinearMotorMovementService
{
    public async ValueTask<bool> MoveUpAsync(
        int linearMotorId,
        LinearMotorMoveRequest request,
        CancellationToken cancellationToken)
    {
        LinearMotorMovementContext? context = await BuildContextAsync(linearMotorId, request, cancellationToken);
        if (context is null)
        {
            return false;
        }

        await actuator.MoveUpAsync(context, cancellationToken);
        return true;
    }

    public async ValueTask<bool> MoveDownAsync(
        int linearMotorId,
        LinearMotorMoveRequest request,
        CancellationToken cancellationToken)
    {
        LinearMotorMovementContext? context = await BuildContextAsync(linearMotorId, request, cancellationToken);
        if (context is null)
        {
            return false;
        }

        await actuator.MoveDownAsync(context, cancellationToken);
        return true;
    }

    private async ValueTask<LinearMotorMovementContext?> BuildContextAsync(
        int linearMotorId,
        LinearMotorMoveRequest request,
        CancellationToken cancellationToken)
    {
        var linearMotor = await linearMotorQueryHandler.GetByIdAsync(linearMotorId, cancellationToken);
        if (linearMotor is null)
        {
            return null;
        }

        var solarPanel = await solarPanelQueryHandler.GetByIdAsync(linearMotor.SolarPanelId, cancellationToken);
        if (solarPanel is null)
        {
            return null;
        }

        var installationSite = await installationSiteQueryHandler.GetByIdAsync(solarPanel.InstallationSiteId, cancellationToken);
        if (installationSite is null)
        {
            return null;
        }

        return new LinearMotorMovementContext(
            linearMotor.Id,
            installationSite.Id,
            installationSite.Latitude,
            installationSite.Longitude,
            linearMotor.MoveUpGpioPin,
            linearMotor.MoveDownGpioPin,
            request.DurationMs);
    }
}
