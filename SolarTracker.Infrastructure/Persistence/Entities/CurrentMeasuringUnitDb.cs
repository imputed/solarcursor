namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class CurrentMeasuringUnitDb
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public SolarPanelDb SolarPanel { get; set; } = null!;

    public string? Name { get; set; }

    public int GpioPin { get; set; }
}
