namespace SolarTracker.Application.Dtos;

public sealed record UpdateTiltMeasuringUnitDto(int Id, int InstallationSiteId, string? Name, int GpioPin);
