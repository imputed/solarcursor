namespace SolarTracker.Application.Dtos.LinearMotor;

public sealed record LinearMotorDto(
    int Id,
    int SolarPanelId,
    string? Name,
    int MoveUpGpioPin,
    int MoveDownGpioPin);
