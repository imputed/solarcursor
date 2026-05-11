namespace SolarTracker.Application.Dtos;

public sealed record CreateLinearMotorDto(
    int SolarPanelId,
    string? Name,
    int MoveUpGpioPin,
    int MoveDownGpioPin);
