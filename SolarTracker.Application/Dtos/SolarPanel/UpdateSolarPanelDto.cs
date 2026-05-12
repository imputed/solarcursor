namespace SolarTracker.Application.Dtos.SolarPanel;

public sealed record UpdateSolarPanelDto(int Id, int InstallationSiteId, string? SerialNumber);
