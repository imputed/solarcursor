using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class InstallationSitePersistenceMapping
{
    public static InstallationSite ToDomain(InstallationSiteDb db, bool loadChildren)
    {
        var panels = loadChildren && db.SolarPanels is not null
            ? db.SolarPanels.Select(panel => SolarPanelPersistenceMapping.ToDomain(panel, loadChildren: true)).ToList()
            : [];

        return new InstallationSite
        {
            Id = db.Id,
            Name = db.Name,
            Latitude = db.Latitude,
            Longitude = db.Longitude,
            TiltMeasuringUnit = loadChildren && db.TiltMeasuringUnit is not null
                ? TiltMeasuringUnitPersistenceMapping.ToDomain(db.TiltMeasuringUnit)
                : null,
            SolarPanels = panels,
        };
    }

    public static InstallationSiteDb ToDb(InstallationSite domain) =>
        new()
        {
            Id = domain.Id,
            Name = domain.Name,
            Latitude = domain.Latitude,
            Longitude = domain.Longitude,
        };

    public static void CopyScalars(InstallationSiteDb target, InstallationSite source)
    {
        target.Name = source.Name;
        target.Latitude = source.Latitude;
        target.Longitude = source.Longitude;
    }
}
