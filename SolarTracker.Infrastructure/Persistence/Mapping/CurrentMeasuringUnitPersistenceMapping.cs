using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class CurrentMeasuringUnitPersistenceMapping
{
    public static CurrentMeasuringUnit ToDomain(CurrentMeasuringUnitDb db) =>
        new()
        {
            Id = db.Id,
            SolarPanelId = db.SolarPanelId,
            Name = db.Name,
            GpioPin = db.GpioPin,
        };

    public static CurrentMeasuringUnitDb ToDb(CurrentMeasuringUnit domain) =>
        new()
        {
            Id = domain.Id,
            SolarPanelId = domain.SolarPanelId,
            Name = domain.Name,
            GpioPin = domain.GpioPin,
        };

    public static void CopyScalars(CurrentMeasuringUnitDb target, CurrentMeasuringUnit source)
    {
        target.SolarPanelId = source.SolarPanelId;
        target.Name = source.Name;
        target.GpioPin = source.GpioPin;
    }
}
