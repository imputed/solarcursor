using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class TiltMeasuringUnitService(ITiltMeasuringUnitRepository repository) : ITiltMeasuringUnitService
{
    public async ValueTask<int> AddAsync(CreateTiltMeasuringUnitDto dto, CancellationToken cancellationToken)
    {
        TiltMeasuringUnit entity = TiltMeasuringUnitMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateTiltMeasuringUnitDto dto, CancellationToken cancellationToken)
    {
        TiltMeasuringUnit entity = TiltMeasuringUnitMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
        => await repository.DeleteAsync(id, cancellationToken);
}
