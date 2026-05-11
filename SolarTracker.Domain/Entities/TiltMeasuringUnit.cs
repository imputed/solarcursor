namespace SolarTracker.Domain.Entities;

public sealed class TiltMeasuringUnit
{
    public int Id { get; set; }

    public int InstallationSiteId { get; set; }

    public string? Name { get; set; }

    public int GpioPin { get; set; }
}
