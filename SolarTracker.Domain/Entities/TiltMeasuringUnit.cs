using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Domain.Entities;

public sealed class TiltMeasuringUnit
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public string? Name { get; set; }

    public int GpioPin { get; set; }

    public ValueTask<TiltMeasurement> GetCurrentPosition(
        ITiltMeasuringUnitPositionReader positionReader,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(positionReader);
        return positionReader.GetCurrentPositionAsync(this, cancellationToken);
    }
}
