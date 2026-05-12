using Microsoft.Extensions.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Logging;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Application.Dtos.InstallationSite;
using SolarTracker.Application.Interfaces.Services;

namespace SolarTracker.Application.Services;

public sealed class InstallationSiteService(
    IInstallationSiteRepository repository,
    IInstallationSiteQueryHandler queryHandler,
    ITiltMeasuringUnitPositionReader tiltMeasuringUnitPositionReader,
    ISteeringCommandReceiver steeringCommandReceiver,
    ISolarOptimalPositionCalculator solarOptimalPositionCalculator,
    TimeProvider timeProvider,
    ILogger<InstallationSiteService> logger) : IInstallationSiteService
{
    public async ValueTask<int> AddAsync(CreateInstallationSiteDto dto, CancellationToken cancellationToken)
    {
        var entity = InstallationSiteMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        ApplicationLog.CreatedInstallationSite(logger, entity.Id, entity.Name);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateInstallationSiteDto dto, CancellationToken cancellationToken)
    {
        var entity = InstallationSiteMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
        ApplicationLog.UpdatedInstallationSite(logger, entity.Id);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        ApplicationLog.DeletedInstallationSite(logger, id);
    }

    public async Task<Result> Optimize(int id, CancellationToken cancellationToken)
    {
        InstallationSite? installationSite = await queryHandler.GetByIdAsync(id, cancellationToken);
        if (installationSite is null)
        {
            ResultError error = SolarTrackerErrorCatalog.InstallationSite.NotFound(id);
            ApplicationLog.InstallationSiteMoveToOptimumFailed(logger, id, error.Code, error.Message);
            return Result.NotFound(error);
        }

        var optimizationResult = await installationSite.OptimizeAsync(solarOptimalPositionCalculator, timeProvider.GetUtcNow(), tiltMeasuringUnitPositionReader, steeringCommandReceiver, cancellationToken);
        if (!optimizationResult.IsSuccess)
        {
            ResultError error = SolarTrackerErrorCatalog.InstallationSite.OptimizationFailure(optimizationResult);
            ApplicationLog.InstallationSiteMoveToOptimumFailed(logger, id, error.Code, error.Message);
            return Result.Failure(error);
        }

        ApplicationLog.InstallationSiteMoveToOptimumCompleted(logger, id, installationSite.SolarPanels.Count);
        return Result.Success();
    }
}