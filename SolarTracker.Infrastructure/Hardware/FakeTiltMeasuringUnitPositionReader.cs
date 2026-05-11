using Microsoft.Extensions.Logging;
using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Entities;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Infrastructure.Hardware;

public sealed class FakeTiltMeasuringUnitPositionReader(
    ILogger<FakeTiltMeasuringUnitPositionReader> logger) : ITiltMeasuringUnitPositionReader
{
    public ValueTask<TiltMeasurement> GetCurrentPositionAsync(
        TiltMeasuringUnit unit,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        logger.LogInformation(
            "Simulating tilt read for measuring unit {TiltMeasuringUnitId} on GPIO pin {GpioPin}.",
            unit.Id,
            unit.GpioPin);

        return ValueTask.FromResult(new TiltMeasurement(45d, DateTime.UtcNow));
    }
}
