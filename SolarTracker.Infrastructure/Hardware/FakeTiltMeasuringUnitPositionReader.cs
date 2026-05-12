using Microsoft.Extensions.Logging;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;
using SolarTracker.Infrastructure.Logging;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class FakeTiltMeasuringUnitPositionReader(
    ILogger<FakeTiltMeasuringUnitPositionReader> logger) : ITiltMeasuringUnitPositionReader
{
    public ValueTask<TiltMeasurement> GetCurrentPositionAsync(
        TiltMeasuringUnit unit,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        InfrastructureLog.SimulatingTiltRead(logger, unit.Id, unit.GpioPin);

        return ValueTask.FromResult(new TiltMeasurement(45d, DateTime.UtcNow));
    }
}
