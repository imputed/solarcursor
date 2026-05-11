using SolarTracker.Domain.Entities;

namespace SolarTracker.Application.Interfaces.Repositories;

public interface ITiltMeasuringUnitRepository
{
    ValueTask AddAsync(TiltMeasuringUnit entity, CancellationToken cancellationToken);

    ValueTask UpdateAsync(TiltMeasuringUnit entity, CancellationToken cancellationToken);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken);
}
