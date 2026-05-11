using SolarTracker.Application.Dtos;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

public static class SolarPanelOptimizationStateMapping
{
    public static SolarPanelOptimizationStateDto ToDto(SolarPanelOptimizationState entity) =>
        new(entity.SolarPanelId, entity.IsEnabled);

    public static SolarPanelOptimizationState ToDomain(
        int solarPanelId,
        UpdateSolarPanelOptimizationStateDto dto) =>
        new()
        {
            SolarPanelId = solarPanelId,
            IsEnabled = dto.IsEnabled,
        };
}
