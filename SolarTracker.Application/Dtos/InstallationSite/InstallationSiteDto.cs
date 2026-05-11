namespace SolarTracker.Application.Dtos;

public sealed record InstallationSiteDto(
    int Id,
    string Name,
    decimal Latitude,
    decimal Longitude,
    TiltMeasuringUnitDto? TiltMeasuringUnit,
    IReadOnlyList<SolarPanelDto> SolarPanels);
