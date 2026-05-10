using SolarTracker.Domain.Entities;
using SolarTracker.Infrastructure.Persistence.Entities;

namespace SolarTracker.Infrastructure.Persistence.Mapping;

internal static class PersistenceMapper
{
    public static InstallationSite ToDomainInstallationSite(InstallationSiteDb db, bool loadChildren)
    {
        var panels = loadChildren && db.SolarPanels is not null
            ? db.SolarPanels.Select(ToDomainSolarPanel).ToList()
            : [];

        var motors = loadChildren && db.LinearMotors is not null
            ? db.LinearMotors.Select(ToDomainLinearMotor).ToList()
            : [];

        return new InstallationSite
        {
            Id = db.Id,
            Name = db.Name,
            SolarPanels = panels,
            LinearMotors = motors,
        };
    }

    public static InstallationSiteDb ToDbInstallationSite(InstallationSite domain)
    {
        return new InstallationSiteDb
        {
            Id = domain.Id,
            Name = domain.Name,
        };
    }

    public static SolarPanel ToDomainSolarPanel(SolarPanelDb db)
    {
        return new SolarPanel
        {
            Id = db.Id,
            InstallationSiteId = db.InstallationSiteId,
            SerialNumber = db.SerialNumber,
        };
    }

    public static SolarPanelDb ToDbSolarPanel(SolarPanel domain)
    {
        return new SolarPanelDb
        {
            Id = domain.Id,
            InstallationSiteId = domain.InstallationSiteId,
            SerialNumber = domain.SerialNumber,
        };
    }

    public static LinearMotor ToDomainLinearMotor(LinearMotorDb db)
    {
        return new LinearMotor
        {
            Id = db.Id,
            InstallationSiteId = db.InstallationSiteId,
            Name = db.Name,
        };
    }

    public static LinearMotorDb ToDbLinearMotor(LinearMotor domain)
    {
        return new LinearMotorDb
        {
            Id = domain.Id,
            InstallationSiteId = domain.InstallationSiteId,
            Name = domain.Name,
        };
    }

    public static void CopyInstallationSiteScalars(InstallationSiteDb target, InstallationSite source)
    {
        target.Name = source.Name;
    }

    public static void CopySolarPanelScalars(SolarPanelDb target, SolarPanel source)
    {
        target.InstallationSiteId = source.InstallationSiteId;
        target.SerialNumber = source.SerialNumber;
    }

    public static void CopyLinearMotorScalars(LinearMotorDb target, LinearMotor source)
    {
        target.InstallationSiteId = source.InstallationSiteId;
        target.Name = source.Name;
    }
}
