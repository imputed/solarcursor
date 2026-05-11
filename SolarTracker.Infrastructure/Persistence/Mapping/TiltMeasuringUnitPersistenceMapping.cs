using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class TiltMeasuringUnitPersistenceMapping
{
    public static TiltMeasuringUnit ToDomain(TiltMeasuringUnitDb db) =>
        new()
        {
            Id = db.Id,
            InstallationSiteId = db.InstallationSiteId,
            Name = db.Name,
            GpioPin = db.GpioPin,
        };

    public static TiltMeasuringUnitDb ToDb(TiltMeasuringUnit domain) =>
        new()
        {
            Id = domain.Id,
            InstallationSiteId = domain.InstallationSiteId,
            Name = domain.Name,
            GpioPin = domain.GpioPin,
        };

    public static void CopyScalars(TiltMeasuringUnitDb target, TiltMeasuringUnit source)
    {
        target.InstallationSiteId = source.InstallationSiteId;
        target.Name = source.Name;
        target.GpioPin = source.GpioPin;
    }
}
