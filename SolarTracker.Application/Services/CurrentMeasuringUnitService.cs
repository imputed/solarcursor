using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Mapping;
using SolarTracker.Application.Logging;
using SolarTracker.Domain.Entities;
using Microsoft.Extensions.Logging;
using SolarTracker.Application.Interfaces.Services;
using SolarTracker.Application.Dtos.CurrentMeasuringUnit;

namespace SolarTracker.Application.Services;

public sealed class CurrentMeasuringUnitService(
    ICurrentMeasuringUnitRepository repository,
    ILogger<CurrentMeasuringUnitService> logger) : ICurrentMeasuringUnitService
{
    public async ValueTask<int> AddAsync(CreateCurrentMeasuringUnitDto dto, CancellationToken cancellationToken)
    {
        CurrentMeasuringUnit entity = CurrentMeasuringUnitMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        ApplicationLog.CreatedCurrentMeasuringUnit(logger, entity.Id, entity.SolarPanelId);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateCurrentMeasuringUnitDto dto, CancellationToken cancellationToken)
    {
        CurrentMeasuringUnit entity = CurrentMeasuringUnitMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
        ApplicationLog.UpdatedCurrentMeasuringUnit(logger, entity.Id);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        ApplicationLog.DeletedCurrentMeasuringUnit(logger, id);
    }
}
