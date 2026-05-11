namespace SolarTracker.Application.Dtos;

public sealed record CreateCurrentMeasuringUnitDto(int SolarPanelId, string? Name, int GpioPin);
