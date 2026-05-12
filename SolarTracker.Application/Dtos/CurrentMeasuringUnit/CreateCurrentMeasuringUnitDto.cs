namespace SolarTracker.Application.Dtos.CurrentMeasuringUnit;

public sealed record CreateCurrentMeasuringUnitDto(int SolarPanelId, string? Name, int GpioPin);
