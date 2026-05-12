using SolarTracker.Application.Dtos.TiltMeasuringUnit;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

public static class TiltMeasuringUnitMapping
{
    public static TiltMeasuringUnitDto ToDto(TiltMeasuringUnit entity) =>
        new(entity.Id, entity.SolarPanelId, entity.Name, entity.GpioPin);

    public static TiltMeasuringUnit ToDomain(CreateTiltMeasuringUnitDto dto) =>
        new() { SolarPanelId = dto.SolarPanelId, Name = dto.Name, GpioPin = dto.GpioPin };

    public static TiltMeasuringUnit ToDomain(UpdateTiltMeasuringUnitDto dto) =>
        new() { Id = dto.Id, SolarPanelId = dto.SolarPanelId, Name = dto.Name, GpioPin = dto.GpioPin };
}
