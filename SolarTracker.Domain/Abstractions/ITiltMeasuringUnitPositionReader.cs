using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Domain.Abstractions;

public interface ITiltMeasuringUnitPositionReader
{
    ValueTask<TiltMeasurement> GetCurrentPositionAsync(
        TiltMeasuringUnit unit,
        CancellationToken cancellationToken);
}
