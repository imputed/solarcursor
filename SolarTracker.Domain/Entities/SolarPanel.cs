namespace SolarTracker.Domain.Entities;

public sealed class SolarPanel
{
    public int Id { get; set; }

    public int InstallationSiteId { get; set; }

    public string? SerialNumber { get; set; }
}
