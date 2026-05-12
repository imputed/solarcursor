using SolarTracker.Domain.Abstractions;
using SolarTracker.Domain.Errors;
using SolarTracker.Domain.ValueObjects;

namespace SolarTracker.Domain.Entities;

public sealed class InstallationSite
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public ICollection<SolarPanel> SolarPanels { get; set; } = [];

    public double GetOptimalPosition(ISolarOptimalPositionCalculator calculator, DateTimeOffset timestamp)
    {
        ArgumentNullException.ThrowIfNull(calculator);
        return calculator.CalculateOptimalPosition(Latitude, Longitude, timestamp);
    }

    public async Task<InstallationSiteOptimizationResult> OptimizeAsync(
        ISolarOptimalPositionCalculator calculator,
        DateTimeOffset timestamp,
        ITiltMeasuringUnitPositionReader positionReader,
        ISteeringCommandReceiver steeringCommandReceiver,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(positionReader);
        ArgumentNullException.ThrowIfNull(steeringCommandReceiver);

        double targetPosition = GetOptimalPosition(calculator, timestamp);
        foreach (SolarPanel solarPanel in SolarPanels.OrderBy(x => x.Id))
        {
            SolarPanelMovementValidationResult validationResult = solarPanel.ValidateSolarPanelForMovement();
            if (validationResult != SolarPanelMovementValidationResult.Valid)
                return InstallationSiteOptimizationResult.SolarPanelValidationFailed(solarPanel.Id, validationResult);

            if (solarPanel.SolarTrackingConfiguration is null)
            {
                throw new InvalidOperationException(
                    DomainTextCatalog.InstallationSite.OptimizationRequiresSolarTrackingConfiguration(solarPanel.Id));
            }

            SolarPanelMoveResult moveResult = await solarPanel.MoveToTargetPositionAsync(
                targetPosition,
                solarPanel.SolarTrackingConfiguration,
                positionReader,
                steeringCommandReceiver,
                cancellationToken);
            if (!moveResult.IsSuccess)
                return InstallationSiteOptimizationResult.SolarPanelMovementFailed(solarPanel.Id, moveResult);
        }

        return InstallationSiteOptimizationResult.Success();
    }

    public async ValueTask<TiltMeasurement> GetPosition(
        ITiltMeasuringUnitPositionReader positionReader,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(positionReader);

        if (SolarPanels.Count == 0)
            throw new InvalidOperationException(DomainTextCatalog.InstallationSite.PositionRequiresSolarPanels());

        List<TiltMeasurement> measurements = new(SolarPanels.Count);
        foreach (SolarPanel solarPanel in SolarPanels.OrderBy(x => x.Id))
            measurements.Add(await solarPanel.GetPosition(positionReader, cancellationToken));

        double averageDegrees = measurements.Average(x => x.Degrees);
        DateTime latestTimestamp = measurements.Max(x => x.Timestamp);
        return new TiltMeasurement(averageDegrees, latestTimestamp);
    }
}
