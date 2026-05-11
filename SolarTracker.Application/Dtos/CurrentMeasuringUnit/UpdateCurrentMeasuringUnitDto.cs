namespace SolarTracker.Application.Dtos;

public sealed record UpdateCurrentMeasuringUnitDto(int Id, int SolarPanelId, string? Name, int GpioPin);
