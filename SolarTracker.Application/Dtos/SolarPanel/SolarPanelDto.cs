namespace SolarTracker.Application.Dtos;

public sealed record SolarPanelDto(
    int Id,
    int InstallationSiteId,
    string? SerialNumber,
    TiltMeasuringUnitDto? TiltMeasuringUnit,
    CurrentMeasuringUnitDto? CurrentMeasuringUnit,
    IReadOnlyList<LinearMotorDto> LinearMotors);
