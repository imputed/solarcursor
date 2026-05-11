namespace SolarTracker.Application.Dtos;

public sealed record LinearMotorDto(
    int Id,
    int SolarPanelId,
    string? Name,
    int MoveUpGpioPin,
    int MoveDownGpioPin);
