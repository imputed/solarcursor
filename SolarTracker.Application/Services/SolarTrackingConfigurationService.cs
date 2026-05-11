using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class SolarTrackingConfigurationService(
    ISolarTrackingConfigurationRepository repository,
    ISolarPanelQueryHandler solarPanelQueryHandler) : ISolarTrackingConfigurationService
{
    public async ValueTask<Result<SolarTrackingConfigurationDto>> GetAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        if (await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken) is null)
            return Result<SolarTrackingConfigurationDto>.NotFound(
                "solar-panel-not-found",
                $"Solar panel {solarPanelId} was not found.");

        SolarTrackingConfiguration entity = await repository.GetBySolarPanelIdAsync(solarPanelId, cancellationToken);
        return Result<SolarTrackingConfigurationDto>.Success(SolarTrackingConfigurationMapping.ToDto(entity));
    }

    public async ValueTask<Result<SolarTrackingConfigurationDto>> UpdateAsync(
        int solarPanelId,
        UpdateSolarTrackingConfigurationDto dto,
        CancellationToken cancellationToken)
    {
        if (await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken) is null)
            return Result<SolarTrackingConfigurationDto>.NotFound(
                "solar-panel-not-found",
                $"Solar panel {solarPanelId} was not found.");

        SolarTrackingConfiguration entity = SolarTrackingConfigurationMapping.ToDomain(solarPanelId, dto);
        SolarTrackingConfiguration updated = await repository.UpsertAsync(entity, cancellationToken);
        return Result<SolarTrackingConfigurationDto>.Success(SolarTrackingConfigurationMapping.ToDto(updated));
    }
}
