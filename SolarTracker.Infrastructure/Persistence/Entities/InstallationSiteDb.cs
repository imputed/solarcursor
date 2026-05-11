namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class InstallationSiteDb
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public TiltMeasuringUnitDb? TiltMeasuringUnit { get; set; }

    public ICollection<SolarPanelDb> SolarPanels { get; set; } = [];
}
