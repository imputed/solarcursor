namespace SolarTracker.Application.Dtos;

public sealed record CreateSolarPanelDto(int InstallationSiteId, string? SerialNumber);
