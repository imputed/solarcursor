namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class InstallationSiteDb
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<SolarPanelDb> SolarPanels { get; set; } = [];

    public ICollection<LinearMotorDb> LinearMotors { get; set; } = [];
}
