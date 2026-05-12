using Microsoft.Extensions.Logging;
using SolarTracker.Application.Logging;
using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Calculators;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Results;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class SolarPanelService(
    ISolarPanelRepository repository,
    ISolarPanelCalculator solarPanelCalculator,
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
        Result<SolarPanelCurrentPositionDto> result = await solarPanelCalculator.GetCurrentPositionAsync(id, cancellationToken);
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

    public async ValueTask<Result<SolarPanelCurrentPositionDto>> MoveToOptimumAsync(
        int id,
        CancellationToken cancellationToken)
    {
        Result<SolarPanelCurrentPositionDto> result = await solarPanelCalculator.MoveToOptimumAsync(id, cancellationToken);
        if (result.IsSuccess)
        {
            ApplicationLog.SolarPanelMoveToOptimumCompleted(
                logger,
                id,
                result.Value.CurrentPosition,
                result.Value.OptimalPosition);
            return result;
        }

        ResultError error = result.Error!.Value;
        ApplicationLog.SolarPanelMoveToOptimumFailed(logger, id, error.Code, error.Message);
        return result;
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        ApplicationLog.DeletedSolarPanel(logger, id);
    }
}
