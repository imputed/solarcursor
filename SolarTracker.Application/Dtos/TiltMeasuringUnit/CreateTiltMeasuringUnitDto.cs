namespace SolarTracker.Application.Dtos;

public sealed record CreateTiltMeasuringUnitDto(int SolarPanelId, string? Name, int GpioPin);
