namespace SolarTracker.Infrastructure.Persistence.Entities;

public sealed class SolarPanelDb
{
    public int Id { get; set; }

    public int InstallationSiteId { get; set; }

    public InstallationSiteDb InstallationSite { get; set; } = null!;

    public string? SerialNumber { get; set; }

    public CurrentMeasuringUnitDb? CurrentMeasuringUnit { get; set; }

    public ICollection<LinearMotorDb> LinearMotors { get; set; } = [];
}
