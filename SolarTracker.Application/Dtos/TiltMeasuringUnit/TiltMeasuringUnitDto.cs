namespace SolarTracker.Application.Dtos;

public sealed record TiltMeasuringUnitDto(int Id, int InstallationSiteId, string? Name, int GpioPin);
