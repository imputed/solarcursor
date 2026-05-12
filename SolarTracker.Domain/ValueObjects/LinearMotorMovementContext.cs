namespace SolarTracker.Domain.ValueObjects;

public sealed record LinearMotorMovementContext(
    int LinearMotorId,
    int InstallationSiteId,
    decimal Latitude,
    decimal Longitude,
    int MoveUpGpioPin,
    int MoveDownGpioPin,
    int DurationMs);
