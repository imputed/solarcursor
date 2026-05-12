using SolarTracker.Application.Dtos.LinearMotor;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

public static class LinearMotorMapping
{
    public static LinearMotorDto ToDto(LinearMotor entity) =>
        new(entity.Id, entity.SolarPanelId, entity.Name, entity.MoveUpGpioPin, entity.MoveDownGpioPin);

    public static LinearMotor ToDomain(CreateLinearMotorDto dto) =>
        new()
        {
            SolarPanelId = dto.SolarPanelId,
            Name = dto.Name,
            MoveUpGpioPin = dto.MoveUpGpioPin,
            MoveDownGpioPin = dto.MoveDownGpioPin,
        };

    public static LinearMotor ToDomain(UpdateLinearMotorDto dto) =>
        new()
        {
            Id = dto.Id,
            SolarPanelId = dto.SolarPanelId,
            Name = dto.Name,
            MoveUpGpioPin = dto.MoveUpGpioPin,
            MoveDownGpioPin = dto.MoveDownGpioPin,
        };
}
