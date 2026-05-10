namespace SolarTracker.Application.Dtos;

public sealed record LinearMotorDto(int Id, int InstallationSiteId, string? Name);

public sealed record CreateLinearMotorDto(int InstallationSiteId, string? Name);

public sealed record UpdateLinearMotorDto(int Id, int InstallationSiteId, string? Name);
