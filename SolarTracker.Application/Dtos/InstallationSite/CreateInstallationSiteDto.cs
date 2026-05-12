namespace SolarTracker.Application.Dtos.InstallationSite;

public sealed record CreateInstallationSiteDto(string Name, decimal Latitude, decimal Longitude);
