namespace SolarTracker.Application.Dtos.TiltMeasuringUnit;

public sealed record CreateTiltMeasuringUnitDto(int SolarPanelId, string? Name, int GpioPin);
