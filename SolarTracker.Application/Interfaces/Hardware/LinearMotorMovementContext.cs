namespace SolarTracker.Application.Interfaces.Hardware;

public sealed record LinearMotorMovementContext(
    int LinearMotorId,
    int InstallationSiteId,
    decimal Latitude,
    decimal Longitude,
    int MoveUpGpioPin,
    int MoveDownGpioPin,
    int DurationMs);
