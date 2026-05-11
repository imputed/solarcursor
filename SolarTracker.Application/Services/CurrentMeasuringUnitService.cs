using SolarTracker.Application.Dtos;
using SolarTracker.Application.Interfaces.Repositories;
using SolarTracker.Application.Mapping;
using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Services;

public sealed class CurrentMeasuringUnitService(ICurrentMeasuringUnitRepository repository) : ICurrentMeasuringUnitService
{
    public async ValueTask<int> AddAsync(CreateCurrentMeasuringUnitDto dto, CancellationToken cancellationToken)
    {
        CurrentMeasuringUnit entity = CurrentMeasuringUnitMapping.ToDomain(dto);
        await repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async ValueTask UpdateAsync(UpdateCurrentMeasuringUnitDto dto, CancellationToken cancellationToken)
    {
        CurrentMeasuringUnit entity = CurrentMeasuringUnitMapping.ToDomain(dto);
        await repository.UpdateAsync(entity, cancellationToken);
    }

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken)
        => await repository.DeleteAsync(id, cancellationToken);
}
