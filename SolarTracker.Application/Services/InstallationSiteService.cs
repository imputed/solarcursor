using Microsoft.Extensions.Logging;
using SolarTracker.Application.Errors;
using SolarTracker.Application.Logging;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Calculators;
using SolarTracker.Application.Interfaces.QueryHandlers;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class InstallationSiteService(
    IInstallationSiteRepository repository,
    IInstallationSiteQueryHandler queryHandler,
    ISolarPanelCalculator solarPanelCalculator,
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

    public async ValueTask<Result<IReadOnlyList<SolarPanelCurrentPositionDto>>> MoveToOptimumAsync(
        int id,
        CancellationToken cancellationToken)
    {
        InstallationSite? installationSite = await queryHandler.GetByIdAsync(id, cancellationToken);
        if (installationSite is null)
        {
            ResultError error = SolarTrackerErrorCatalog.InstallationSite.NotFound(id);
            ApplicationLog.InstallationSiteMoveToOptimumFailed(logger, id, error.Code, error.Message);
            return Result<IReadOnlyList<SolarPanelCurrentPositionDto>>.NotFound(error);
        }

        List<SolarPanelCurrentPositionDto> solarPanelPositions = new(installationSite.SolarPanels.Count);
        foreach (SolarPanel solarPanel in installationSite.SolarPanels.OrderBy(x => x.Id))
        {
            Result<SolarPanelCurrentPositionDto> result =
                await solarPanelCalculator.MoveToOptimumAsync(solarPanel.Id, cancellationToken);
            if (result.IsSuccess)
            {
                solarPanelPositions.Add(result.Value);
                continue;
            }

            ResultError error = result.Error!.Value;
            ApplicationLog.InstallationSiteMoveToOptimumFailed(logger, id, error.Code, error.Message);
            return result.IsNotFound
                ? Result<IReadOnlyList<SolarPanelCurrentPositionDto>>.NotFound(error)
                : Result<IReadOnlyList<SolarPanelCurrentPositionDto>>.Failure(error);
        }

        ApplicationLog.InstallationSiteMoveToOptimumCompleted(logger, id, solarPanelPositions.Count);
        return Result<IReadOnlyList<SolarPanelCurrentPositionDto>>.Success(solarPanelPositions);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        ApplicationLog.DeletedInstallationSite(logger, id);
    }
}