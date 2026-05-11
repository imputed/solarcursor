namespace SolarTracker.Application.Dtos;

public sealed record CreateTiltMeasuringUnitDto(int InstallationSiteId, string? Name, int GpioPin);
