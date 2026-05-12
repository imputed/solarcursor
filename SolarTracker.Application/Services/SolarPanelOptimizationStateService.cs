using Microsoft.Extensions.Logging;
using SolarTracker.Application.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;
using SolarTracker.Application.Dtos.SolarPanelOptimizationState;

namespace SolarTracker.Application.Services;

public sealed class SolarPanelOptimizationStateService(
    ISolarPanelOptimizationStateRepository repository,
    ISolarPanelQueryHandler solarPanelQueryHandler,
    ILogger<SolarPanelOptimizationStateService> logger) : ISolarPanelOptimizationStateService
{
    public async ValueTask<Result<SolarPanelOptimizationStateDto>> GetAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        if (await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken) is null)
        {
            ApplicationLog.SolarPanelNotFoundForOptimizationState(logger, solarPanelId);
            return Result<SolarPanelOptimizationStateDto>.NotFound(SolarTrackerErrorCatalog.SolarPanel.NotFound(solarPanelId));
        }

        SolarPanelOptimizationState entity = await repository.GetBySolarPanelIdAsync(solarPanelId, cancellationToken);
        SolarPanelOptimizationStateDto dto = SolarPanelOptimizationStateMapping.ToDto(entity);
        ApplicationLog.RetrievedSolarPanelOptimizationState(logger, solarPanelId, dto.IsEnabled);
        return Result<SolarPanelOptimizationStateDto>.Success(dto);
    }

    public async ValueTask<Result<SolarPanelOptimizationStateDto>> UpdateAsync(
        int solarPanelId,
        UpdateSolarPanelOptimizationStateDto dto,
        CancellationToken cancellationToken)
    {
        if (await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken) is null)
        {
            ApplicationLog.SolarPanelNotFoundForOptimizationState(logger, solarPanelId);
            return Result<SolarPanelOptimizationStateDto>.NotFound(SolarTrackerErrorCatalog.SolarPanel.NotFound(solarPanelId));
        }

        SolarPanelOptimizationState entity = SolarPanelOptimizationStateMapping.ToDomain(solarPanelId, dto);
        SolarPanelOptimizationState updated = await repository.UpsertAsync(entity, cancellationToken);
        SolarPanelOptimizationStateDto result = SolarPanelOptimizationStateMapping.ToDto(updated);
        ApplicationLog.UpdatedSolarPanelOptimizationState(logger, solarPanelId, result.IsEnabled);
        return Result<SolarPanelOptimizationStateDto>.Success(result);
    }
}
