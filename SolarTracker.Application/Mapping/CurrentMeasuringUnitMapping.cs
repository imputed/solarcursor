using SolarTracker.Application.Dtos.CurrentMeasuringUnit;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

public static class CurrentMeasuringUnitMapping
{
    public static CurrentMeasuringUnitDto ToDto(CurrentMeasuringUnit entity) =>
        new(entity.Id, entity.SolarPanelId, entity.Name, entity.GpioPin);

    public static CurrentMeasuringUnit ToDomain(CreateCurrentMeasuringUnitDto dto) =>
        new() { SolarPanelId = dto.SolarPanelId, Name = dto.Name, GpioPin = dto.GpioPin };

    public static CurrentMeasuringUnit ToDomain(UpdateCurrentMeasuringUnitDto dto) =>
        new() { Id = dto.Id, SolarPanelId = dto.SolarPanelId, Name = dto.Name, GpioPin = dto.GpioPin };
}
