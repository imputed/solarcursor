using SolarTracker.Application.Dtos;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

public static class SolarTrackingConfigurationMapping
{
    public static SolarTrackingConfigurationDto ToDto(SolarTrackingConfiguration entity) =>
        new(
            entity.SolarPanelId,
            entity.PositionThresholdDegrees,
            entity.StepDurationMs,
            entity.MaxAdjustmentSteps);

    public static SolarTrackingConfiguration ToDomain(int solarPanelId, UpdateSolarTrackingConfigurationDto dto) =>
        new()
        {
            SolarPanelId = solarPanelId,
            PositionThresholdDegrees = dto.PositionThresholdDegrees,
            StepDurationMs = dto.StepDurationMs,
            MaxAdjustmentSteps = dto.MaxAdjustmentSteps,
        };
}
