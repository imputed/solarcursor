namespace SolarTracker.Application.Dtos;

public sealed record UpdateSolarPanelDto(int Id, int InstallationSiteId, string? SerialNumber);
