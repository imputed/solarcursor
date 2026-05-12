namespace SolarTracker.Application.Dtos.LinearMotor;

public sealed record CreateLinearMotorDto(
    int SolarPanelId,
    string? Name,
    int MoveUpGpioPin,
    int MoveDownGpioPin);
