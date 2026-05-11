using SolarTracker.Application.Dtos;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

public static class SolarPanelMapping
{
    public static SolarPanelDto ToDto(SolarPanel entity) =>
        new(
            entity.Id,
            entity.InstallationSiteId,
            entity.SerialNumber,
            entity.CurrentMeasuringUnit is null ? null : CurrentMeasuringUnitMapping.ToDto(entity.CurrentMeasuringUnit),
            entity.LinearMotors.Select(LinearMotorMapping.ToDto).ToList());

    public static SolarPanel ToDomain(CreateSolarPanelDto dto) =>
        new()
        {
            InstallationSiteId = dto.InstallationSiteId,
            SerialNumber = dto.SerialNumber,
            CurrentMeasuringUnit = null,
            LinearMotors = [],
        };

    public static SolarPanel ToDomain(UpdateSolarPanelDto dto) =>
        new()
        {
            Id = dto.Id,
            InstallationSiteId = dto.InstallationSiteId,
            SerialNumber = dto.SerialNumber,
            CurrentMeasuringUnit = null,
            LinearMotors = [],
        };
}
