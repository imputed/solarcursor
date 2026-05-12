namespace SolarTracker.Application.Dtos.TiltMeasuringUnit;

public sealed record TiltMeasuringUnitDto(int Id, int SolarPanelId, string? Name, int GpioPin);
