namespace SolarTracker.Application.Dtos.CurrentMeasuringUnit;

public sealed record CurrentMeasuringUnitDto(int Id, int SolarPanelId, string? Name, int GpioPin);
