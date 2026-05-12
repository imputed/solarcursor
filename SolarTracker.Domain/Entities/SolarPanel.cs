using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Errors;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Domain.Entities;

public sealed class SolarPanel
{
    public int Id { get; set; }

    public int InstallationSiteId { get; set; }

    public string? SerialNumber { get; set; }

    public SolarTrackingConfiguration? SolarTrackingConfiguration { get; set; }

    public TiltMeasuringUnit? TiltMeasuringUnit { get; set; }

    public CurrentMeasuringUnit? CurrentMeasuringUnit { get; set; }

    public ICollection<LinearMotor> LinearMotors { get; set; } = [];

    public async ValueTask<TiltMeasurement> GetPosition(
        ITiltMeasuringUnitPositionReader positionReader,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(positionReader);

        if (TiltMeasuringUnit is null)
            throw new InvalidOperationException(DomainTextCatalog.SolarPanel.PositionRequiresTiltMeasuringUnit());

        return await TiltMeasuringUnit.GetCurrentPosition(positionReader, cancellationToken);
    }
}
