namespace SolarTracker.Application.Dtos;

public sealed record InstallationSiteDto(
    int Id,
    string Name,
    IReadOnlyList<SolarPanelDto> SolarPanels,
    IReadOnlyList<LinearMotorDto> LinearMotors);

public sealed record CreateInstallationSiteDto(string Name);

public sealed record UpdateInstallationSiteDto(int Id, string Name);
