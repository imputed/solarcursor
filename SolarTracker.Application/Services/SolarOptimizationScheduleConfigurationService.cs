using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class SolarOptimizationScheduleConfigurationService(
    ISolarOptimizationScheduleConfigurationRepository repository) : ISolarOptimizationScheduleConfigurationService
{
    public async ValueTask<SolarOptimizationScheduleConfigurationDto> GetAsync(CancellationToken cancellationToken)
    {
        SolarOptimizationScheduleConfiguration entity = await repository.GetAsync(cancellationToken);
        return SolarOptimizationScheduleConfigurationMapping.ToDto(entity);
    }

    public async ValueTask<SolarOptimizationScheduleConfigurationDto> UpdateAsync(
        UpdateSolarOptimizationScheduleConfigurationDto dto,
        CancellationToken cancellationToken)
    {
        SolarOptimizationScheduleConfiguration entity = SolarOptimizationScheduleConfigurationMapping.ToDomain(dto);
        SolarOptimizationScheduleConfiguration updated = await repository.UpsertAsync(entity, cancellationToken);
        return SolarOptimizationScheduleConfigurationMapping.ToDto(updated);
    }
}
