namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class LinearMotorDb
{
    public int Id { get; set; }

    public int InstallationSiteId { get; set; }

    public InstallationSiteDb InstallationSite { get; set; } = null!;

    public string? Name { get; set; }
}
