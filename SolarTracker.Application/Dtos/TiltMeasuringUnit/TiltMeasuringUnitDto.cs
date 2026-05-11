namespace SolarTracker.Application.Dtos;

public sealed record TiltMeasuringUnitDto(int Id, int SolarPanelId, string? Name, int GpioPin);
