namespace SolarTracker.Application.Dtos;

public sealed record CurrentMeasuringUnitDto(int Id, int SolarPanelId, string? Name, int GpioPin);
