using SolarTracker.Application.Dtos;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

internal static class DomainToDtoMapper
{
    public static InstallationSiteDto ToDto(InstallationSite entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.SolarPanels.Select(ToSolarPanelDto).ToList(),
            entity.LinearMotors.Select(ToLinearMotorDto).ToList());

    public static SolarPanelDto ToSolarPanelDto(SolarPanel entity) =>
        new(entity.Id, entity.InstallationSiteId, entity.SerialNumber);

    public static LinearMotorDto ToLinearMotorDto(LinearMotor entity) =>
        new(entity.Id, entity.InstallationSiteId, entity.Name);

    public static InstallationSite ToDomain(CreateInstallationSiteDto dto) =>
        new()
        {
            Name = dto.Name,
            SolarPanels = [],
            LinearMotors = [],
        };

    public static InstallationSite ToDomain(UpdateInstallationSiteDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            SolarPanels = [],
            LinearMotors = [],
        };

    public static SolarPanel ToDomain(CreateSolarPanelDto dto) =>
        new() { InstallationSiteId = dto.InstallationSiteId, SerialNumber = dto.SerialNumber };

    public static SolarPanel ToDomain(UpdateSolarPanelDto dto) =>
        new()
        {
            Id = dto.Id,
            InstallationSiteId = dto.InstallationSiteId,
            SerialNumber = dto.SerialNumber,
        };

    public static LinearMotor ToDomain(CreateLinearMotorDto dto) =>
        new() { InstallationSiteId = dto.InstallationSiteId, Name = dto.Name };

    public static LinearMotor ToDomain(UpdateLinearMotorDto dto) =>
        new()
        {
            Id = dto.Id,
            InstallationSiteId = dto.InstallationSiteId,
            Name = dto.Name,
        };
}
