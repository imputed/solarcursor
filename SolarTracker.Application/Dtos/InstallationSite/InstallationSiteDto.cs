using SolarTracker.Application.Dtos.SolarPanel;

namespace SolarTracker.Application.Dtos.InstallationSite;

public sealed record InstallationSiteDto(
    int Id,
    string Name,
    decimal Latitude,
    decimal Longitude,
    IReadOnlyList<SolarPanelDto> SolarPanels);
