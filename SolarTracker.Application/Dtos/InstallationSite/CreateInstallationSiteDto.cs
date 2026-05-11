namespace SolarTracker.Application.Dtos;

public sealed record CreateInstallationSiteDto(string Name, decimal Latitude, decimal Longitude);
