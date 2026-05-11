using SolarTracker.Application.Dtos;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

public static class TiltMeasuringUnitMapping
{
    public static TiltMeasuringUnitDto ToDto(TiltMeasuringUnit entity) =>
        new(entity.Id, entity.InstallationSiteId, entity.Name, entity.GpioPin);

    public static TiltMeasuringUnit ToDomain(CreateTiltMeasuringUnitDto dto) =>
        new() { InstallationSiteId = dto.InstallationSiteId, Name = dto.Name, GpioPin = dto.GpioPin };

    public static TiltMeasuringUnit ToDomain(UpdateTiltMeasuringUnitDto dto) =>
        new() { Id = dto.Id, InstallationSiteId = dto.InstallationSiteId, Name = dto.Name, GpioPin = dto.GpioPin };
}
