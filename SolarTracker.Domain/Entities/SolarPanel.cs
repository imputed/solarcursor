namespace SolarTracker.Domain.Entities;

public sealed class SolarPanel
{
    public int Id { get; set; }

    public int InstallationSiteId { get; set; }

    public string? SerialNumber { get; set; }

    public CurrentMeasuringUnit? CurrentMeasuringUnit { get; set; }

    public ICollection<LinearMotor> LinearMotors { get; set; } = [];
}
