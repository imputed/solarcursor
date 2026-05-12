using SolarTracker.Application.Dtos.CurrentMeasuringUnit;
using SolarTracker.Application.Dtos.LinearMotor;
using SolarTracker.Application.Dtos.TiltMeasuringUnit;

namespace SolarTracker.Application.Dtos.SolarPanel;

public sealed record SolarPanelDto(
    int Id,
    int InstallationSiteId,
    string? SerialNumber,
    TiltMeasuringUnitDto? TiltMeasuringUnit,
    CurrentMeasuringUnitDto? CurrentMeasuringUnit,
    IReadOnlyList<LinearMotorDto> LinearMotors);
