namespace SolarTracker.Application.Dtos;

public sealed record UpdateInstallationSiteDto(int Id, string Name, decimal Latitude, decimal Longitude);
