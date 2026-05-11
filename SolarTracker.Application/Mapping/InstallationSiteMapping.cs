using SolarTracker.Application.Dtos;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

public static class InstallationSiteMapping
{
    public static InstallationSiteDto ToDto(InstallationSite entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.Latitude,
            entity.Longitude,
            entity.TiltMeasuringUnit is null ? null : TiltMeasuringUnitMapping.ToDto(entity.TiltMeasuringUnit),
            entity.SolarPanels.Select(SolarPanelMapping.ToDto).ToList());

    public static InstallationSite ToDomain(CreateInstallationSiteDto dto) =>
        new()
        {
            Name = dto.Name,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            TiltMeasuringUnit = null,
            SolarPanels = [],
        };

    public static InstallationSite ToDomain(UpdateInstallationSiteDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            TiltMeasuringUnit = null,
            SolarPanels = [],
        };
}
