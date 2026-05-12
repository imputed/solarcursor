using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Domain.Entities;

public sealed class LinearMotor
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public string? Name { get; set; }

    public int MoveUpGpioPin { get; set; }

    public int MoveDownGpioPin { get; set; }

    public ValueTask MoveUpAsync(
        ILinearMotorActuator actuator,
        int installationSiteId,
        decimal latitude,
        decimal longitude,
        int durationMs,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(actuator);
        return actuator.MoveUpAsync(
            CreateMovementContext(installationSiteId, latitude, longitude, durationMs),
            cancellationToken);
    }

    public ValueTask MoveDownAsync(
        ILinearMotorActuator actuator,
        int installationSiteId,
        decimal latitude,
        decimal longitude,
        int durationMs,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(actuator);
        return actuator.MoveDownAsync(
            CreateMovementContext(installationSiteId, latitude, longitude, durationMs),
            cancellationToken);
    }

    private LinearMotorMovementContext CreateMovementContext(
        int installationSiteId,
        decimal latitude,
        decimal longitude,
        int durationMs) =>
        new(
            Id,
            installationSiteId,
            latitude,
            longitude,
            MoveUpGpioPin,
            MoveDownGpioPin,
            durationMs);
}
