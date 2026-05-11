namespace SolarTracker.Application.Dtos;

public readonly record struct SolarPanelCurrentPositionDto(
    int SolarPanelId,
    double OptimalPosition,
    double CurrentPosition);
