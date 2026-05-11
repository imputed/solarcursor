namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class TiltMeasuringUnitDb
{
    public int Id { get; set; }

    public int InstallationSiteId { get; set; }

    public InstallationSiteDb InstallationSite { get; set; } = null!;

    public string? Name { get; set; }

    public int GpioPin { get; set; }
}
