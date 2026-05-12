using Microsoft.Extensions.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Logging;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class SolarPanelService(
    ISolarPanelRepository repository,
    ISolarPanelQueryHandler solarPanelQueryHandler,
    IInstallationSiteQueryHandler installationSiteQueryHandler,
    ITiltMeasuringUnitPositionReader tiltMeasuringUnitPositionReader,
    ISolarOptimalPositionCalculator solarOptimalPositionCalculator,
    TimeProvider timeProvider,
    ILogger<SolarPanelService> logger) : ISolarPanelService
{
    public async ValueTask<int> AddAsync(CreateSolarPanelDto dto, CancellationToken cancellationToken)
    {
        SolarPanel entity = SolarPanelMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        ApplicationLog.CreatedSolarPanel(logger, entity.Id, entity.InstallationSiteId);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateSolarPanelDto dto, CancellationToken cancellationToken)
    {
        SolarPanel entity = SolarPanelMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
        ApplicationLog.UpdatedSolarPanel(logger, entity.Id);
    }

    public async ValueTask<Result<SolarPanelCurrentPositionDto>> GetCurrentPositionAsync(
        int id,
        CancellationToken cancellationToken)
    {
        Result<(SolarPanel SolarPanel, InstallationSite InstallationSite)> contextResult =
            await BuildCurrentPositionContextAsync(id, cancellationToken);
        Result<SolarPanelCurrentPositionDto> result;
        if (!contextResult.IsSuccess)
        {
            ResultError contextError = contextResult.Error!.Value;
            result = Result<SolarPanelCurrentPositionDto>.NotFound(contextError);
        }
        else
        {
            TiltMeasurement measurement = await contextResult.Value.SolarPanel.GetPosition(
                tiltMeasuringUnitPositionReader,
                cancellationToken);
            double optimalPosition = contextResult.Value.InstallationSite.GetOptimalPosition(
                solarOptimalPositionCalculator,
                timeProvider.GetUtcNow());
            result = Result<SolarPanelCurrentPositionDto>.Success(
                new SolarPanelCurrentPositionDto(
                    contextResult.Value.SolarPanel.Id,
                    optimalPosition,
                    measurement.Degrees));
        }

        if (result.IsSuccess)
        {
            ApplicationLog.RetrievedSolarPanelCurrentPosition(
                logger,
                id,
                result.Value.CurrentPosition,
                result.Value.OptimalPosition);
            return result;
        }

        ResultError error = result.Error!.Value;
        ApplicationLog.SolarPanelCurrentPositionUnavailable(logger, id, error.Code, error.Message);
        return result;
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        ApplicationLog.DeletedSolarPanel(logger, id);
    }

    private async ValueTask<Result<(SolarPanel SolarPanel, InstallationSite InstallationSite)>> BuildCurrentPositionContextAsync(
        int solarPanelId,
        CancellationToken cancellationToken)
    {
        SolarPanel? solarPanel = await solarPanelQueryHandler.GetByIdAsync(solarPanelId, cancellationToken);
        if (solarPanel is null)
            return Result<(SolarPanel SolarPanel, InstallationSite InstallationSite)>.NotFound(
                SolarTrackerErrorCatalog.SolarPanel.NotFound(solarPanelId));

        if (solarPanel.TiltMeasuringUnit is null)
            return Result<(SolarPanel SolarPanel, InstallationSite InstallationSite)>.Failure(
                SolarTrackerErrorCatalog.SolarPanel.TiltMeasuringUnitMissing(solarPanelId));

        InstallationSite? installationSite =
            await installationSiteQueryHandler.GetByIdAsync(solarPanel.InstallationSiteId, cancellationToken);
        if (installationSite is null)
            return Result<(SolarPanel SolarPanel, InstallationSite InstallationSite)>.NotFound(
                SolarTrackerErrorCatalog.InstallationSite.NotFound(solarPanel.InstallationSiteId));

        return Result<(SolarPanel SolarPanel, InstallationSite InstallationSite)>.Success((solarPanel, installationSite));
    }
}
