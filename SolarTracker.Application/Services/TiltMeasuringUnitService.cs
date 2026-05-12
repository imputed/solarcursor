using Microsoft.Extensions.Logging;
using SolarTracker.Application.Logging;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;
using SolarTracker.Application.Dtos.TiltMeasuringUnit;
using SolarTracker.Application.Interfaces.Services;

namespace SolarTracker.Application.Services;

public sealed class TiltMeasuringUnitService(
    ITiltMeasuringUnitRepository repository,
    ILogger<TiltMeasuringUnitService> logger) : ITiltMeasuringUnitService
{
    public async ValueTask<int> AddAsync(CreateTiltMeasuringUnitDto dto, CancellationToken cancellationToken)
    {
        TiltMeasuringUnit entity = TiltMeasuringUnitMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        ApplicationLog.CreatedTiltMeasuringUnit(logger, entity.Id, entity.SolarPanelId);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateTiltMeasuringUnitDto dto, CancellationToken cancellationToken)
    {
        TiltMeasuringUnit entity = TiltMeasuringUnitMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
        ApplicationLog.UpdatedTiltMeasuringUnit(logger, entity.Id);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(id, cancellationToken);
        ApplicationLog.DeletedTiltMeasuringUnit(logger, id);
    }
}
