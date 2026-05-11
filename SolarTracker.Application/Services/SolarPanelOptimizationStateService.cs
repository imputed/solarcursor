using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class SolarPanelOptimizationStateService(
    ISolarPanelOptimizationStateRepository repository,
    ISolarPanelQueryHandler solarPanelQueryHandler) : ISolarPanelOptimizationStateService
{
    public async ValueTask<Result<SolarPanelOptimizationStateDto>> GetAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        if (await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken) is null)
            return Result<SolarPanelOptimizationStateDto>.NotFound(
                "solar-panel-not-found",
                $"Solar panel {solarPanelId} was not found.");

        SolarPanelOptimizationState entity = await repository.GetBySolarPanelIdAsync(solarPanelId, cancellationToken);
        return Result<SolarPanelOptimizationStateDto>.Success(SolarPanelOptimizationStateMapping.ToDto(entity));
    }

    public async ValueTask<Result<SolarPanelOptimizationStateDto>> UpdateAsync(
        int solarPanelId,
        UpdateSolarPanelOptimizationStateDto dto,
        CancellationToken cancellationToken)
    {
        if (await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken) is null)
            return Result<SolarPanelOptimizationStateDto>.NotFound(
                "solar-panel-not-found",
                $"Solar panel {solarPanelId} was not found.");

        SolarPanelOptimizationState entity = SolarPanelOptimizationStateMapping.ToDomain(solarPanelId, dto);
        SolarPanelOptimizationState updated = await repository.UpsertAsync(entity, cancellationToken);
        return Result<SolarPanelOptimizationStateDto>.Success(SolarPanelOptimizationStateMapping.ToDto(updated));
    }
}
