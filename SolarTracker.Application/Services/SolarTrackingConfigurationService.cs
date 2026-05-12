using Microsoft.Extensions.Logging;
using SolarTracker.Application.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;
using SolarTracker.Application.Dtos.SolarTrackingConfiguration;

namespace SolarTracker.Application.Services;

public sealed class SolarTrackingConfigurationService(
    ISolarTrackingConfigurationRepository repository,
    ISolarPanelQueryHandler solarPanelQueryHandler,
    ILogger<SolarTrackingConfigurationService> logger) : ISolarTrackingConfigurationService
{
    public async ValueTask<Result<SolarTrackingConfigurationDto>> GetAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        if (await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken) is null)
        {
            ApplicationLog.SolarPanelNotFoundForTrackingConfiguration(logger, solarPanelId);
            return Result<SolarTrackingConfigurationDto>.NotFound(SolarTrackerErrorCatalog.SolarPanel.NotFound(solarPanelId));
        }

        SolarTrackingConfiguration entity = await repository.GetBySolarPanelIdAsync(solarPanelId, cancellationToken);
        SolarTrackingConfigurationDto dto = SolarTrackingConfigurationMapping.ToDto(entity);
        ApplicationLog.RetrievedSolarTrackingConfiguration(logger, solarPanelId);
        return Result<SolarTrackingConfigurationDto>.Success(dto);
    }

    public async ValueTask<Result<SolarTrackingConfigurationDto>> UpdateAsync(
        int solarPanelId,
        UpdateSolarTrackingConfigurationDto dto,
        CancellationToken cancellationToken)
    {
        if (await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken) is null)
        {
            ApplicationLog.SolarPanelNotFoundForTrackingConfiguration(logger, solarPanelId);
            return Result<SolarTrackingConfigurationDto>.NotFound(SolarTrackerErrorCatalog.SolarPanel.NotFound(solarPanelId));
        }

        SolarTrackingConfiguration entity = SolarTrackingConfigurationMapping.ToDomain(solarPanelId, dto);
        SolarTrackingConfiguration updated = await repository.UpsertAsync(entity, cancellationToken);
        SolarTrackingConfigurationDto result = SolarTrackingConfigurationMapping.ToDto(updated);
        ApplicationLog.UpdatedSolarTrackingConfiguration(
            logger,
            solarPanelId,
            result.PositionThresholdDegrees,
            result.StepDurationMs,
            result.MaxAdjustmentSteps);
        return Result<SolarTrackingConfigurationDto>.Success(result);
    }
}
