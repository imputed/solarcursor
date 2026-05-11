namespace SolarTracker.Domain.Entities;

public sealed class CurrentMeasuringUnit
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public string? Name { get; set; }

    public int GpioPin { get; set; }
}
