using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class SolarPanelPersistenceMapping
{
    public static SolarPanel ToDomain(SolarPanelDb db, bool loadChildren) =>
        new()
        {
            Id = db.Id,
            InstallationSiteId = db.InstallationSiteId,
            SerialNumber = db.SerialNumber,
            SolarTrackingConfiguration = loadChildren && db.SolarTrackingConfiguration is not null
                ? SolarTrackingConfigurationPersistenceMapping.ToDomain(db.SolarTrackingConfiguration)
                : null,
            TiltMeasuringUnit = loadChildren && db.TiltMeasuringUnit is not null
                ? TiltMeasuringUnitPersistenceMapping.ToDomain(db.TiltMeasuringUnit)
                : null,
            CurrentMeasuringUnit = loadChildren && db.CurrentMeasuringUnit is not null
                ? CurrentMeasuringUnitPersistenceMapping.ToDomain(db.CurrentMeasuringUnit)
                : null,
            LinearMotors = loadChildren && db.LinearMotors is not null
                ? db.LinearMotors.Select(LinearMotorPersistenceMapping.ToDomain).ToList()
                : [],
        };

    public static SolarPanelDb ToDb(SolarPanel domain) =>
        new()
        {
            Id = domain.Id,
            InstallationSiteId = domain.InstallationSiteId,
            SerialNumber = domain.SerialNumber,
        };

    public static void CopyScalars(SolarPanelDb target, SolarPanel source)
    {
        target.InstallationSiteId = source.InstallationSiteId;
        target.SerialNumber = source.SerialNumber;
    }
}
