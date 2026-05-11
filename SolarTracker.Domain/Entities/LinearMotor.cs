namespace SolarTracker.Domain.Entities;

public sealed class LinearMotor
{
    public int Id { get; set; }

    public int SolarPanelId { get; set; }

    public string? Name { get; set; }

    public int MoveUpGpioPin { get; set; }

    public int MoveDownGpioPin { get; set; }
}
