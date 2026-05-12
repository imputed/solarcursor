using SolarTracker.Application.Dtos.SolarOptimizationScheduleConfiguration;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Mapping;

public static class SolarOptimizationScheduleConfigurationMapping
{
    public static SolarOptimizationScheduleConfigurationDto ToDto(SolarOptimizationScheduleConfiguration entity) =>
        new(entity.IntervalMinutes);

    public static SolarOptimizationScheduleConfiguration ToDomain(
        UpdateSolarOptimizationScheduleConfigurationDto dto) =>
        new()
        {
            Id = SolarOptimizationScheduleConfiguration.SingletonId,
            IntervalMinutes = dto.IntervalMinutes,
        };
}
