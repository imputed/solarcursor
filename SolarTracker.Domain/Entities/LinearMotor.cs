namespace SolarTracker.Domain.Entities;

public sealed class LinearMotor
{
    public int Id { get; set; }

    public int InstallationSiteId { get; set; }

    public string? Name { get; set; }
}
