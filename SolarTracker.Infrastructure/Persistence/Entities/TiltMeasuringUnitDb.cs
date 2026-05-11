namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class TiltMeasuringUnitDb
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public SolarPanelDb SolarPanel { get; set; } = null!;

    public string? Name { get; set; }

    public int GpioPin { get; set; }
}
