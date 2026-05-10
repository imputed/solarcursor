namespace SolarTracker.Domain.Entities;

public sealed class InstallationSite
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<SolarPanel> SolarPanels { get; set; } = [];

    public ICollection<LinearMotor> LinearMotors { get; set; } = [];
}
