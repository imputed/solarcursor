namespace SolarTracker.Application.Dtos;

public sealed record SolarPanelDto(int Id, int InstallationSiteId, string? SerialNumber);

public sealed record CreateSolarPanelDto(int InstallationSiteId, string? SerialNumber);

public sealed record UpdateSolarPanelDto(int Id, int InstallationSiteId, string? SerialNumber);
