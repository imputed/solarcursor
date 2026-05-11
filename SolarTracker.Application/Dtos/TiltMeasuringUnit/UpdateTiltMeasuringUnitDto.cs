namespace SolarTracker.Application.Dtos;

public sealed record UpdateTiltMeasuringUnitDto(int Id, int SolarPanelId, string? Name, int GpioPin);
