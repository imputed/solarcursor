namespace SolarTracker.Application.Dtos.CurrentMeasuringUnit;

public sealed record UpdateCurrentMeasuringUnitDto(int Id, int SolarPanelId, string? Name, int GpioPin);
