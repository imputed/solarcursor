namespace SolarTracker.Application.Dtos;

public sealed record SolarPanelDto(
    int Id,
    int InstallationSiteId,
    string? SerialNumber,
    CurrentMeasuringUnitDto? CurrentMeasuringUnit,
    IReadOnlyList<LinearMotorDto> LinearMotors);
