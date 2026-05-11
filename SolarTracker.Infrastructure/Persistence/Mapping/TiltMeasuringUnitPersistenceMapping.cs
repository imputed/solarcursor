using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class TiltMeasuringUnitPersistenceMapping
{
    public static TiltMeasuringUnit ToDomain(TiltMeasuringUnitDb db) =>
        new()
        {
            Id = db.Id,
            SolarPanelId = db.SolarPanelId,
            Name = db.Name,
            GpioPin = db.GpioPin,
        };

    public static TiltMeasuringUnitDb ToDb(TiltMeasuringUnit domain) =>
        new()
        {
            Id = domain.Id,
            SolarPanelId = domain.SolarPanelId,
            Name = domain.Name,
            GpioPin = domain.GpioPin,
        };

    public static void CopyScalars(TiltMeasuringUnitDb target, TiltMeasuringUnit source)
    {
        target.SolarPanelId = source.SolarPanelId;
        target.Name = source.Name;
        target.GpioPin = source.GpioPin;
    }
}
