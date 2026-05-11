namespace SolarTracker.Application.Dtos;

public sealed record UpdateLinearMotorDto(
    int Id,
    int SolarPanelId,
    string? Name,
    int MoveUpGpioPin,
    int MoveDownGpioPin);
