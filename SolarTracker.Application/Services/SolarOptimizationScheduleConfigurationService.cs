using Microsoft.Extensions.Logging;
using SolarTracker.Application.Logging;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;
using SolarTracker.Application.Dtos.SolarOptimizationScheduleConfiguration;

namespace SolarTracker.Application.Services;

public sealed class SolarOptimizationScheduleConfigurationService(
    ISolarOptimizationScheduleConfigurationRepository repository,
    ILogger<SolarOptimizationScheduleConfigurationService> logger) : ISolarOptimizationScheduleConfigurationService
{
    public async ValueTask<SolarOptimizationScheduleConfigurationDto> GetAsync(CancellationToken cancellationToken)
    {
        SolarOptimizationScheduleConfiguration entity = await repository.GetAsync(cancellationToken);
        SolarOptimizationScheduleConfigurationDto dto = SolarOptimizationScheduleConfigurationMapping.ToDto(entity);
        ApplicationLog.RetrievedSolarOptimizationScheduleConfiguration(logger, dto.IntervalMinutes);
        return dto;
    }

    public async ValueTask<SolarOptimizationScheduleConfigurationDto> UpdateAsync(
        UpdateSolarOptimizationScheduleConfigurationDto dto,
        CancellationToken cancellationToken)
    {
        SolarOptimizationScheduleConfiguration entity = SolarOptimizationScheduleConfigurationMapping.ToDomain(dto);
        SolarOptimizationScheduleConfiguration updated = await repository.UpsertAsync(entity, cancellationToken);
        SolarOptimizationScheduleConfigurationDto result = SolarOptimizationScheduleConfigurationMapping.ToDto(updated);
        ApplicationLog.UpdatedSolarOptimizationScheduleConfiguration(logger, result.IntervalMinutes);
        return result;
    }
}
