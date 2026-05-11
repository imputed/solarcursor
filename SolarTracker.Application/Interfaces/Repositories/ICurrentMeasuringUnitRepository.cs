using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface ICurrentMeasuringUnitRepository
{
    ValueTask AddAsync(CurrentMeasuringUnit entity, CancellationToken cancellationToken);

    ValueTask UpdateAsync(CurrentMeasuringUnit entity, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
