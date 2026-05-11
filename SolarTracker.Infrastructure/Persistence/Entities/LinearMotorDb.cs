namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class LinearMotorDb
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public SolarPanelDb SolarPanel { get; set; } = null!;

    public string? Name { get; set; }

    public int MoveUpGpioPin { get; set; }

    public int MoveDownGpioPin { get; set; }
}
