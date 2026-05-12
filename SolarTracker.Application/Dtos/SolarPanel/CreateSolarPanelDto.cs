namespace SolarTracker.Application.Dtos.SolarPanel;

public sealed record CreateSolarPanelDto(int InstallationSiteId, string? SerialNumber);
